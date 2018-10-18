using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// EventController sealed class, provides API endpoints for calendar events.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/calendar/[controller]")]
    public sealed class EventController : ApiController
    {
        #region Variables
        private readonly IDataSource _datasource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EventController object.
        /// </summary>
        /// <param name="datasource"></param>
        public EventController(IDataSource datasource)
        {
            _datasource = datasource;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a calendar event for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the event.</param>
        /// <returns>An event for the specified 'id'.</returns>
        [HttpGet("{id}")]
        public IActionResult Event(int id)
        {
            var cevent = _datasource.Events.Get(id);
            return Ok(cevent);
        }

        /// <summary>
        /// Returns an array of events for the calendar specified by the 'id'.
        /// </summary>
        /// <param name="id">The calendar id.</param>
        /// <param name="startOn">The start date for the calendar to return.  Defaults to now.</param>
        /// <param name="endOn">The end date for the calendar to return.</param>
        /// <returns>An array of events.</returns>
        [HttpGet("/[area]/calendar/{id}/events")]
        public IActionResult EventsForCalendar(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            var cevents = _datasource.Events.Get(id, start, end);
            return cevents.Count() != 0 ? Ok(cevents) : (IActionResult)NoContent();
        }
        #endregion
    }
}
