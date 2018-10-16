using Fosol.Schedule.DAL.Interfaces;
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
            var calendar = this.Find(id);

            return this.Source.Mapper.Map<Models.Event>(calendar);
        }

        /// <summary>
        /// Get the events for the specified calendar, within the specified date range.
        /// Valdiates whether the current user is authorized to view the calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Models.Event> Get(int calendarId, DateTime startOn, DateTime endOn)
        {
            // Convert datetime to utc.
            var start = startOn.ToUniversalTime();
            var end = endOn.ToUniversalTime();

            //var events = this.Source.Context.Events.Where(e => e.CalendarId == calendarId && e.StartOn >= start && e.EndOn <= end).Select(e => this.Source.Mapper.Map<Models.Event>(e));
            //return events;
            return null;
        }
        #endregion
    }
}
