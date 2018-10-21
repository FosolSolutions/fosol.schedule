using Fosol.Core.Exceptions;
using Fosol.Core.Extensions.Enumerable;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// OpeningService sealed class, provides a way to manage openings in the datasource.
    /// </summary>
    public sealed class OpeningService : UpdatableService<Entities.Opening, Models.Opening>, IOpeningService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal OpeningService(IDataSource source) : base(source)
        {
            //Authenticated();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the opening for the specified 'id'.
        /// Validates whether the current user is authorized to view the opening.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Opening Get(int id)
        {
            this.VerifyPrincipal();
            var accountId = this.GetAccountId();
            return this.Map(this.Find((set) => set.SingleOrDefault(o => o.Id == id && o.Activity.Event.Calendar.AccountId == accountId)));
        }

        /// <summary>
        /// The participant is apply for the opening.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="participants"></param>
        /// <returns></returns>
        public Models.Opening Apply(Models.Opening opening, params Models.Participant[] participants)
        {
            if (opening == null) throw new ArgumentNullException(nameof(opening));

            // If none are specified select the current user's participant.
            if (participants == null || participants.Count() == 0)
            {
                var participantId = this.GetParticipantId() ?? throw new NotAuthorizedException();
                participants = new[] { new Models.Participant() { Id = participantId } };
            }

            var entity = this.Find((set) => set.Include(o => o.Applications).Include(o => o.Participants).Include(o => o.Criteria).ThenInclude(c => c.Criteria).Include(o => o.Activity).ThenInclude(a => a.Event).SingleOrDefault(o => o.Id == opening.Id));
            if (Convert.ToBase64String(entity.RowVersion) != opening.RowVersion) throw new InvalidOperationException($"The opening has been updated recently.  Please resync your version before applying."); // TODO: Resource file for text.

            var participantCount = entity.Participants.Count();
            if (entity.MaxParticipants <= participantCount) throw new InvalidOperationException($"The opening has been filled, {participantCount} of {entity.MaxParticipants} applications have been approved.");
            if (entity.MaxParticipants < participantCount + participants.Count()) throw new InvalidOperationException($"The opening only has {entity.MaxParticipants - participantCount} positions available.  You have submitted {participants.Count()} applications.");

            var applicationIds = participants.Select(a => a.Id).ToArray();
            if (entity.Participants.Any(a => applicationIds.Contains(a.ParticipantId))) throw new InvalidOperationException($"Cannot apply for the same opening after the participant has been approved.");

            var openingCriteria = entity.Criteria.ToArray().Select(c => (Criteria)c.Criteria);
            var activityCriteria = this.Context.ActivityCriteria.Include(ac => ac.Criteria).Where(ac => ac.ActivityId == entity.ActivityId).ToArray().Select(ac => (Criteria)ac.Criteria);
            var eventCriteria = this.Context.EventCriteria.Include(ec => ec.Criteria).Where(ec => ec.EventId == entity.Activity.EventId).ToArray().Select(ac => (Criteria)ac.Criteria);
            var calendarCriteria = this.Context.CalendarCriteria.Include(cc => cc.Criteria).Where(cc => cc.CalendarId == entity.Activity.Event.CalendarId).ToArray().Select(ac => (Criteria)ac.Criteria);
            var criteria = openingCriteria.Concat(activityCriteria).Concat(eventCriteria).Concat(calendarCriteria);

            if (entity.ApplicationProcess == ApplicationProcess.Review)
            {
                if (entity.Applications.Any(a => applicationIds.Contains(a.ParticipantId))) throw new InvalidOperationException($"Cannot apply for an opening more than once.  An application has already been accepted.");

                foreach (var application in participants)
                {
                    var participant = this.Find<Participant>((set) => set.Include(p => p.Attributes).ThenInclude(a => a.Attribute).SingleOrDefault(p => p.Id == application.Id));

                    // Validate criteria
                    if (!criteria.All(c => c.Validate(participant.Attributes.Select(a => a.Attribute).ToArray()))) throw new InvalidOperationException($"The participant '{participant.DisplayName}' does not have the required attributes to apply to this opening.");

                    var opa = new OpeningParticipantApplication(entity, participant);
                    entity.Applications.Add(opa);
                    this.Context.OpeningParticipantApplications.Add(opa);
                }
            }
            else
            {
                // Auto accepted to the opening.
                foreach (var application in participants)
                {
                    var participant = this.Find<Participant>((set) => set.Include(p => p.Attributes).ThenInclude(a => a.Attribute).SingleOrDefault(p => p.Id == application.Id));

                    // Validate criteria
                    if (!criteria.All(c => c.Validate(participant.Attributes.Select(a => a.Attribute).ToArray()))) throw new InvalidOperationException($"The participant '{participant.DisplayName}' does not have the required attributes to apply to this opening.");

                    var op = new OpeningParticipant(entity, participant);
                    entity.Participants.Add(op);
                    this.Context.OpeningParticipants.Add(op);
                }
            }

            this.Update(entity);
            return this.Map(entity);
        }

        /// <summary>
        /// The participant is unapplying to the opening.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="participants"></param>
        /// <returns></returns>
        public Models.Opening Unapply(Models.Opening opening, params Models.Participant[] participants)
        {
            if (opening == null) throw new ArgumentNullException(nameof(opening));

            // If none are specified select the current user's participant.
            if (participants == null || participants.Count() == 0)
            {
                var participantId = this.GetParticipantId();
                var participant = this.Find<Participant>((set) => set.Find(participantId));
                participants = new[] { new Models.Participant() { Id = participant.Id } };
            }

            var entity = this.Find((set) => set.Include(o => o.Applications).Include(o => o.Participants).SingleOrDefault(o => o.Id == opening.Id));
            if (Convert.ToBase64String(entity.RowVersion) != opening.RowVersion) throw new InvalidOperationException($"The opening has been updated recently.  Please resync your version before unapplying.");

            var applicationIds = participants.Select(a => a.Id).ToArray();
            var applications = entity.Applications.Where(a => applicationIds.Contains(a.ParticipantId));
            applications.ForEach(a => entity.Applications.Remove(a));

            var activeParticipants = entity.Participants.Where(a => applicationIds.Contains(a.ParticipantId));
            activeParticipants.ForEach(p => entity.Participants.Remove(p));

            this.Update(entity);
            return this.Map(entity);
        }
        #endregion
    }
}
