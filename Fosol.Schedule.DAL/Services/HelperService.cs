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
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Models.Calendar AddEcclesialCalendar(string name = "Victoria Christadelphian Ecclesia", string description = "The Victoria Christadelphian ecclesial calendar")
        {
            if (this.IsPrincipalAParticipant) throw new NotAuthorizedException();

            var userId = this.Source.Principal.GetNameIdentifier().Value.ConvertTo<int>();
            var user = this.Context.Users.Find(userId) ?? throw new NotAuthorizedException();

            var account = this.Context.Accounts.First(a => a.OwnerId == userId);
            var events = new List<Event>(365);
            var calendar = new Calendar(account, name)
            {
                Description = description,
                AddedById = userId
            };

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
            var criteria = new List<CriteriaObject>() { member, brother, sister, pianist, presider, exhorter, reader, server, prayer, door, emblems };

            // Sunday Memorial
            var jan1st = new DateTime(DateTime.Now.Year, 1, 1);
            var sunday = jan1st.DayOfWeek == DayOfWeek.Sunday ? jan1st : jan1st.AddDays(7 - (int)jan1st.DayOfWeek);
            while (sunday.Year == DateTime.Now.Year)
            {
                var cevent = new Event(calendar, "Memorial Meeting", sunday.AddHours(11), sunday.AddHours(13))
                {
                    Description = "Sunday memorial meeting.",
                    AddedById = userId
                };
                var aPreside = new Activity(cevent, "Preside") { AddedById = userId };
                aPreside.Openings.Add(new Opening(aPreside, "Presider", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPreside.Criteria.Add(new ActivityCriteria(aPreside, presider));
                cevent.Activities.Add(aPreside);

                var aExhort = new Activity(cevent, "Exhortation") { AddedById = userId };
                aExhort.Openings.Add(new Opening(aExhort, "Exhort", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aExhort.Criteria.Add(new ActivityCriteria(aExhort, exhorter));
                cevent.Activities.Add(aExhort);

                var aDoor = new Activity(cevent, "Door") { AddedById = userId };
                aDoor.Openings.Add(new Opening(aDoor, "Door", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aDoor.Criteria.Add(new ActivityCriteria(aDoor, door));
                cevent.Activities.Add(aDoor);

                var aReadings = new Activity(cevent, "Readings") { AddedById = userId };
                aReadings.Openings.Add(new Opening(aReadings, "1st Reading", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aReadings.Openings.Add(new Opening(aReadings, "2nd Reading", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aReadings.Criteria.Add(new ActivityCriteria(aReadings, reader));
                cevent.Activities.Add(aReadings);

                var aPrayers = new Activity(cevent, "Prayers") { AddedById = userId };
                aPrayers.Openings.Add(new Opening(aPrayers, "Bread", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPrayers.Openings.Add(new Opening(aPrayers, "Wine", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPrayers.Openings.Add(new Opening(aPrayers, "Close", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPrayers.Criteria.Add(new ActivityCriteria(aPrayers, prayer));
                cevent.Activities.Add(aPrayers);

                var aPianist = new Activity(cevent, "Pianist") { AddedById = userId };
                aPianist.Openings.Add(new Opening(aPianist, "Pianist", 1, 1, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aPianist.Criteria.Add(new ActivityCriteria(aPianist, pianist));
                cevent.Activities.Add(aPianist);

                var aServe = new Activity(cevent, "Serve") { AddedById = userId };
                aServe.Openings.Add(new Opening(aServe, "Servers", 4, 4, OpeningType.Application, ApplicationProcess.Accept) { AddedById = userId });
                aServe.Criteria.Add(new ActivityCriteria(aServe, server));
                cevent.Activities.Add(aServe);

                calendar.Events.Add(cevent);
                sunday = sunday.AddDays(7);
            }

            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    this.Context.Criteria.AddRange(criteria);
                    this.Context.Calendars.Add(calendar);
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
