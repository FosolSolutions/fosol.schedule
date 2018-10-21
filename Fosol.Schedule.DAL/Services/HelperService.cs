using Fosol.Core.Exceptions;
using Fosol.Core.Extensions.Principals;
using Fosol.Core.Extensions.Strings;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Schedule.DAL.Services
{
    public sealed class HelperService : IHelperService
    {
        #region Variables
        private readonly IDataSource _source;
        #endregion

        #region Properties
        /// <summary>
        /// get - The datasource.
        /// </summary>
        private DataSource Source { get { return _source as DataSource; } }

        /// <summary>
        /// get - The DbContext used to communicate with the datasource.
        /// </summary>
        private ScheduleContext Context { get { return this.Source.Context; } }

        /// <summary>
        /// get - Whether the user is currently authenticated.
        /// </summary>
        private bool IsAuthenticated { get { return this.Source.Principal.Identity.IsAuthenticated; } }

        /// <summary>
        /// get - Whether the current logged in principal is a participant or not.
        /// </summary>
        /// <returns></returns>
        private bool IsPrincipalAParticipant
        {
            get
            {
                bool.TryParse(this.Source.Principal.GetParticipant()?.Value, out bool participant);
                return participant;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a HelperService object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="source"></param>
        internal HelperService(IDataSource source)
        {
            _source = source;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates and adds a new ecclesial calendar with default events, activities, openings and criteria.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        public Models.Calendar GenerateEcclesialSchedule(int calendarId, DateTime? startOn = null, DateTime? endOn = null)
        {
            if (this.IsPrincipalAParticipant) throw new NotAuthorizedException();

            var userId = this.Source.Principal.GetNameIdentifier().Value.ConvertTo<int>();
            var user = this.Context.Users.Include(u => u.Info).SingleOrDefault(u => u.Id == userId) ?? throw new NotAuthorizedException();
            var calendar = this.Context.Calendars.SingleOrDefault(c => c.Id == calendarId) ?? throw new NoContentException(typeof(Calendar));

            // TODO: Need generic fucntion to test permission to allow editing calendar.
            var ownsAccount = this.Context.Accounts.Any(a => a.Id == calendar.AccountId && a.OwnerId == userId);
            if (!ownsAccount) throw new NotAuthorizedException();

            // Determine date range.
            var start = startOn ?? DateTime.UtcNow.Date;
            var end = endOn ?? start.AddDays(365);
            var days = (end - start).Days;

            var events = new List<Event>(days);

            var member = new CriteriaObject("Member", true) { AddedById = userId };
            var brother = new CriteriaObject("Brother", true) { AddedById = userId };
            var sister = new CriteriaObject("Sister", true) { AddedById = userId };
            var presider = new CriteriaObject(new CriteriaValue("Skill", "Preside"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var pianist = new CriteriaObject(new CriteriaValue("Skill", "Pianist"), (Criteria)member) { AddedById = userId };
            var exhorter = new CriteriaObject(new CriteriaValue("Skill", "Exhort"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var reader = new CriteriaObject(new CriteriaValue("Skill", "Read"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var server = new CriteriaObject(new CriteriaValue("Skill", "Serve"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var prayer = new CriteriaObject(new CriteriaValue("Skill", "Pray"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var door = new CriteriaObject(new CriteriaValue("Skill", "Doorman"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var emblems = new CriteriaObject(new CriteriaValue("Skill", "Emblems"), (Criteria)member) { AddedById = userId };
            var lecturer = new CriteriaObject(new CriteriaValue("Skill", "Lecturer"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var study = new CriteriaObject(new CriteriaValue("Skill", "Bible Class"), (Criteria)brother, (Criteria)member) { AddedById = userId };
            var criteria = new List<CriteriaObject>() { member, brother, sister, pianist, presider, exhorter, reader, server, prayer, door, emblems, lecturer };

            // Sunday Memorial
            var sunday = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(7 - (int)start.DayOfWeek);
            while (sunday <= end)
            {
                // Memorial

                var memorial = new Event(calendar, "Memorial Meeting", sunday.AddHours(11), sunday.AddHours(13))
                {
                    Description = "Sunday memorial meeting.",
                    AddedById = userId
                };
                var aPreside = new Activity(memorial, "Preside") { AddedById = userId };
                aPreside.Openings.Add(new Opening(aPreside, "Presider", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPreside.Criteria.Add(new ActivityCriteria(aPreside, presider));
                memorial.Activities.Add(aPreside);

                var aExhort = new Activity(memorial, "Exhortation") { AddedById = userId };
                aExhort.Openings.Add(new Opening(aExhort, "Exhort", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aExhort.Criteria.Add(new ActivityCriteria(aExhort, exhorter));
                memorial.Activities.Add(aExhort);

                var aDoor = new Activity(memorial, "Door") { AddedById = userId };
                aDoor.Openings.Add(new Opening(aDoor, "Door", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aDoor.Criteria.Add(new ActivityCriteria(aDoor, door));
                memorial.Activities.Add(aDoor);

                var aReadings = new Activity(memorial, "Readings") { AddedById = userId };
                aReadings.Openings.Add(new Opening(aReadings, "1st Reading", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aReadings.Openings.Add(new Opening(aReadings, "2nd Reading", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aReadings.Criteria.Add(new ActivityCriteria(aReadings, reader));
                memorial.Activities.Add(aReadings);

                var aPrayers = new Activity(memorial, "Prayers") { AddedById = userId };
                aPrayers.Openings.Add(new Opening(aPrayers, "Bread", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPrayers.Openings.Add(new Opening(aPrayers, "Wine", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPrayers.Openings.Add(new Opening(aPrayers, "Close", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPrayers.Criteria.Add(new ActivityCriteria(aPrayers, prayer));
                memorial.Activities.Add(aPrayers);

                var aPianist = new Activity(memorial, "Pianist") { AddedById = userId };
                aPianist.Openings.Add(new Opening(aPianist, "Pianist", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPianist.Criteria.Add(new ActivityCriteria(aPianist, pianist));
                memorial.Activities.Add(aPianist);

                var aServe = new Activity(memorial, "Serve") { AddedById = userId };
                aServe.Openings.Add(new Opening(aServe, "Servers", 4, 4, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aServe.Criteria.Add(new ActivityCriteria(aServe, server));
                memorial.Activities.Add(aServe);

                calendar.Events.Add(memorial);

                // Lecture
                var lecture = new Event(calendar, "Bible Talk", sunday.AddHours(19), sunday.AddHours(20))
                {
                    Description = "Sunday night Bible talk.",
                    AddedById = userId
                };
                var aPreside2 = new Activity(lecture, "Preside") { AddedById = userId };
                aPreside2.Openings.Add(new Opening(aPreside2, "Presider", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPreside2.Criteria.Add(new ActivityCriteria(aPreside2, presider));
                lecture.Activities.Add(aPreside2);

                var aLecture = new Activity(lecture, "Lecture") { AddedById = userId };
                aLecture.Openings.Add(new Opening(aLecture, "Speaker", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aLecture.Criteria.Add(new ActivityCriteria(aLecture, lecturer));
                lecture.Activities.Add(aLecture);

                calendar.Events.Add(lecture);

                sunday = sunday.AddDays(7);
            }

            // Bible Class
            var thursday = start.DayOfWeek <= DayOfWeek.Thursday ? start.AddDays(4 - (int)start.DayOfWeek) : start.AddDays(7 + (int)start.DayOfWeek - 4);
            while (thursday <= end)
            {
                // Lecture
                var bibleClass = new Event(calendar, "Bible Class", thursday.AddHours(19), thursday.AddHours(20))
                {
                    Description = "Sunday night Bible talk.",
                    AddedById = userId
                };
                var aPreside = new Activity(bibleClass, "Preside") { AddedById = userId };
                aPreside.Openings.Add(new Opening(aPreside, "Presider", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPreside.Criteria.Add(new ActivityCriteria(aPreside, presider));
                bibleClass.Activities.Add(aPreside);

                var aSpeak = new Activity(bibleClass, "Speak") { AddedById = userId };
                aSpeak.Openings.Add(new Opening(aSpeak, "Speaker", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aSpeak.Criteria.Add(new ActivityCriteria(aSpeak, study));
                bibleClass.Activities.Add(aSpeak);

                calendar.Events.Add(bibleClass);

                thursday = thursday.AddDays(7);
            }

            // Create a participant record for the user.
            var participant = new Participant(calendar, user) { AddedById = userId };

            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    this.Context.Criteria.AddRange(criteria);
                    this.Context.SaveChanges();

                    this.Context.Calendars.Update(calendar);
                    this.Context.SaveChanges();

                    this.Context.Participants.Add(participant);
                    this.Context.SaveChanges();

                    transaction.Commit();
                }
                catch (DbUpdateException)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return this.Source.UpdateMapper.Map<Models.Calendar>(calendar);
        }
        #endregion
    }
}
