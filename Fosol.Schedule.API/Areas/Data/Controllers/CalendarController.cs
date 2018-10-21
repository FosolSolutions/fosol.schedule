using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// CalendarController class, provides API endpoints for calendars.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/[controller]")]
    public sealed class CalendarController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CalendarController object.
        /// </summary>
        /// <param name="dataSource"></param>
        public CalendarController(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns an array of all the calendars for the current user.
        /// </summary>
        /// <param name="page">The page number (default: 1).</param>
        /// <returns>An array calendar JSON data objects.</returns>
        [HttpGet("/[area]/calendars")]
        public IActionResult GetCalendars(int page)
        {
            var skip = page <= 0 ? 0 : page - 1;
            // TODO: Configurable 'take'.
            // TODO: no tracking.
            var calendars = _dataSource.Calendars.Get(skip, 10);

            return calendars.Count() != 0 ? Ok(calendars) : (IActionResult)NoContent();
        }

        /// <summary>
        /// Returns the specified calendar and its events for the current week (or timespan).
        /// </summary>
        /// <param name="id">The primary key to identify the calendar.</param>
        /// <param name="startOn">The start date for the calendar to return.  Defaults to now.</param>
        /// <param name="endOn">The end date for the calendar to return.</param>
        /// <returns>A calendar JSON data object with all events within the specified date range.</returns>
        [HttpGet("{id}")]
        public IActionResult GetCalendar(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            // TODO: no tracking.
            var calendar = _dataSource.Calendars.Get(id, start, end);
            return calendar != null ? Ok(calendar) : (IActionResult)NoContent();
        }

        /// <summary>
        /// Add the specified calendar to the datasource.
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddCalendar([FromBody] Models.Calendar calendar)
        {
            _dataSource.Calendars.Add(calendar);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetCalendar), new { calendar.Id }), calendar);
        }

        /// <summary>
        /// Update the specified calendar in the datasource.
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateCalendar([FromBody] Models.Calendar calendar)
        {
            _dataSource.Calendars.Update(calendar);
            _dataSource.CommitTransaction();

            return Ok(calendar);
        }

        /// <summary>
        /// Delete the specified calendar from the datasource.
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteCalendar([FromBody] Models.Calendar calendar)
        {
            _dataSource.Calendars.Remove(calendar);
            _dataSource.CommitTransaction();

            return Ok();
        }

        /// <summary>
        /// Creates and adds a new calendar for the specified ecclesia.
        /// </summary>
        /// <param name="calendar">The calendar to add to the datasource. JSON data object in the body of the request.</param>
        /// <returns>The calendar that was added to the datasource.</returns>
        [HttpPost("ecclesia")]
        public IActionResult AddEcclesialCalendar([FromBody] Models.Calendar calendar)
        {
            var result = _dataSource.Helper.AddEcclesialCalendar(calendar.Name, calendar.Description);

            return Ok(result);
        }
        #endregion
    }
}
