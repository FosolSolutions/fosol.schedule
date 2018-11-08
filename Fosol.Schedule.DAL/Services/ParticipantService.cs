using Fosol.Core.Exceptions;
using Fosol.Core.Extensions.Enumerable;
using Fosol.Schedule.DAL.Helpers;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// ParticipantService sealed class, provides a way to manage participants in the datasource.
    /// </summary>
    public sealed class ParticipantService : UpdatableService<Participant, Models.Participant>, IParticipantService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParticipantService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal ParticipantService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the participant for the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Models.Participant Get(Guid key)
        {
            var participant = this.Context.Participants.FirstOrDefault(p => p.Key == key) ?? throw new NoContentException(typeof(Models.Participant));

            return this.Map(participant);
        }

        /// <summary>
        /// Get the participant for the specified 'id'.
        /// Validates whether the current user is authorized to view the participant.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Participant Get(int id)
        {
            return this.Map(this.Find((set) => set.Include(p => p.Attributes).ThenInclude(p => p.Attribute).SingleOrDefault(p => p.Id == id)));
        }

        /// <summary>
        /// Get all the participants in the specified calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<Models.Participant> GetForCalendar(int calendarId, int skip = 0, int take = 20)
        {
            // TODO: verify access to calendar.
            var participants = this.Context.Participants.Where(p => p.CalendarId == calendarId).OrderBy(p => p.LastName).ThenBy(p => p.FirstName).Skip(skip).Take(take).Select(p => this.Map(p)).ToArray();

            return participants;
        }

        /// <summary>
        /// Update the specified participant in the datasource.
        /// </summary>
        /// <param name="model"></param>
        public override void Update(Models.Participant model)
        {
            // Strip out collections, they must be saved independently.
            model.Attributes = null;
            model.ContactInfo = null;

            base.Update(model);
        }

        public void AddAttribute(Models.Participant participant, Models.Attribute attribute)
        {
        }

        /// <summary>
        /// Get the claimed identity of the participant for the specified 'participantId'.
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaims(int participantId)
        {
            var participant = this.Find((set) => set.Include(p => p.Attributes).ThenInclude(a => a.Attribute).Include(p => p.Calendar).ThenInclude(c => c.Account).SingleOrDefault(p => p.Id == participantId));
            var email = this.Context.ContactInfo.FirstOrDefault(ci => ci.Participants.Any(pci => pci.ParticipantId == participantId) && ci.Type == ContactInfoType.Email);

            var claims = new List<Claim>(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{participant.Key}", typeof(Guid).FullName, "CoEvent"), // TODO: Namespace constant
                new Claim(ClaimTypes.Email, email?.Value ?? "", typeof(string).FullName, "CoEvent"),
                new Claim(ClaimTypes.Name, $"{participant.FirstName} {participant.LastName}", typeof(string).FullName, "CoEvent"),
                new Claim(ClaimTypes.GivenName, participant.FirstName, typeof(string).FullName, "CoEvent"),
                new Claim(ClaimTypes.Surname, participant.LastName, typeof(string).FullName, "CoEvent"),
                new Claim(ClaimTypes.Gender, $"{participant.Gender}", typeof(Gender).FullName, "CoEvent"),
                new Claim("Participant", $"{participant.Id}", typeof(int).FullName, "CoEvent"),
                new Claim("Calendar", $"{participant.CalendarId}", typeof(int).FullName, "CoEvent"),
                new Claim("Account", $"{participant.Calendar.AccountId}", typeof(int).FullName, "CoEvent")
            });

            foreach (var attr in participant.Attributes)
            {
                claims.Add(new Claim(attr.Attribute.Key, attr.Attribute.Value, attr.Attribute.ValueType, "CoEvent"));
            }

            return claims;
        }
        #endregion
    }
}
