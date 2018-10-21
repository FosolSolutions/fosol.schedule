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
        [HttpGet("{id}", Name = "GetEvent")]
        public IActionResult GetEvent(int id)
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
        public IActionResult GetEventsForCalendar(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            var cevents = _datasource.Events.Get(id, start, end);
            return cevents.Count() != 0 ? Ok(cevents) : (IActionResult)NoContent();
        }

        /// <summary>
        /// Adds the new event to the datasource.
        /// </summary>
        /// <param name="cevent"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEvent([FromBody] Models.Event cevent)
        {
            _datasource.Events.Add(cevent);
            _datasource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetEvent), new { cevent.Id }), cevent);
        }

        /// <summary>
        /// Updates the specified event in the datasource.
        /// </summary>
        /// <param name="cevent"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateEvent([FromBody] Models.Event cevent)
        {
            _datasource.Events.Update(cevent);
            _datasource.CommitTransaction();

            return Ok(cevent);
        }

        /// <summary>
        /// Deletes the specified event from the datasource.
        /// </summary>
        /// <param name="cevent"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteEvent([FromBody] Models.Event cevent)
        {
            _datasource.Events.Remove(cevent);
            _datasource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
