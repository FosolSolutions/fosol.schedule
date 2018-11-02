using Fosol.Schedule.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// EventService sealed class, provides a way to manage events in the datasource.
    /// </summary>
    public sealed class EventService : UpdatableService<Entities.Event, Models.Event>, IEventService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EventService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal EventService(IDataSource source) : base(source)
        {
            //Authenticated();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the event for the specified 'id'.
        /// Validates whether the current user is authorized to view the event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Event Get(int id)
        {
            // TODO: Is user allowed ot see event?
            return this.Map(this.Find((set) => set.Include(e => e.Criteria).Include(e => e.Activities).SingleOrDefault(e => e.Id == id)));
        }

        /// <summary>
        /// Get the events for the specified calendar, within the specified date range.
        /// Validates whether the current user is authorized to view the calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Models.Event> Get(int calendarId, DateTime startOn, DateTime endOn)
        {
            // TODO: Is user allowed ot see calendar?

            // Convert datetime to utc.
            var start = startOn.ToUniversalTime();
            var end = endOn.ToUniversalTime();

            var events = this.Context.Events.Include(e => e.Criteria).Where(e => e.CalendarId == calendarId && e.StartOn >= start && e.EndOn <= end).Select(e => this.Map(e)).ToArray();
            return events;
        }

        /// <summary>
        /// Get the event Ids for the specified schedule.
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public IEnumerable<int> GetEventIdsForSchedule(int scheduleId)
        {
            return this.Context.Schedules.Where(s => s.Id == scheduleId).SelectMany(s => s.Events.Select(e => e.EventId)).ToArray();
        }
        #endregion
    }
}
