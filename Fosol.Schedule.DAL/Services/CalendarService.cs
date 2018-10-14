using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// CalendarService sealed class, provides a way to manage calendars in the datasource.
    /// </summary>
    public sealed class CalendarService : UpdatableService<Entities.Calendar, Models.Calendar>, ICalendarService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CalendarService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal CalendarService(DataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get all the calendars owned by the current user.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<Models.Calendar> Get(int skip, int take)
        {
            var account = this.Context.Accounts.Find(1);
            var calendar = new Entities.Calendar(account, "test") { AddedById = 1 };
            this.Context.Calendars.Add(calendar);
            this.Context.SaveChanges();
            return this.Source.Context.Calendars.Skip(skip).Take(take).ToArray().Select(c => this.Source.Mapper.Map<Entities.Calendar, Models.Calendar>(c)).ToArray();
        }

        /// <summary>
        /// Get the calendar for the specified 'id'.
        /// Validates whether the current user is authorized to view the calendar.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Calendar Get(int id)
        {
            var calendar = this.Find(id);

            return this.Source.Mapper.Map<Models.Calendar>(calendar);
        }

        /// <summary>
        /// Get the calendar for the specified 'id'.
        /// Valdiates whether the current user is authorized to view the calendar.
        /// Includes events for the specified timeframe.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startOn"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Models.Calendar Get(int id, DateTime startOn, DateTime endOn)
        {
            // Convert datetime to utc.
            var start = startOn.ToUniversalTime();
            var end = endOn.ToUniversalTime();

            var calendar = Get(id);
            //var events = this.Source.Context.Events.Where(e => e.CalendarId == id && e.StartOn >= start && e.EndOn <= end);

            //calendar.Events = events.Select(e => this.Source.Mapper.Map<Models.Event>(e));

            return calendar;
        }
        #endregion
    }
}
