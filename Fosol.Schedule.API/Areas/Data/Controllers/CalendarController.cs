using Fosol.Schedule.API.Helpers;
using Fosol.Schedule.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// CalendarController class, provides API endpoints for calendars.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/[controller]")]
    public sealed class CalendarController : Controller
    {
        #region Variables
        private readonly List<Calendar> _calendars;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CalendarController object.
        /// </summary>
        public CalendarController()
        {
            _calendars = CalendarHelper.CreateCalendars();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns an array of all the calendars for the current user.
        /// </summary>
        /// <returns>An array calendar.</returns>
        [HttpGet("/[area]/calendars")]
        public IActionResult Calendars()
        {
            var calendars = _calendars.Select(c => new
            {
                c.Id,
                c.Key,
                c.Name,
                c.Description,
                SelfUrl = $"/data/calendar/{c.Id}"
            }).ToArray();
            return Ok(calendars);
        }

        /// <summary>
        /// Returns the specified calendar and its events for the current week (or timespan).
        /// </summary>
        /// <param name="id">The primary key to identify the calendar.</param>
        /// <param name="startDate">The start date for the calendar to return.  Defaults to now.</param>
        /// <param name="endDate">The end date for the calendar to return.</param>
        /// <returns>A calendar with all events within the specified date range.</returns>
        [HttpGet("{id}")]
        public IActionResult Calendar(int id, DateTime? startDate = null, DateTime? endDate = null)
        {
            var start = startDate ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endDate ?? start.AddDays(7);
            var calendar = _calendars.Where(c => c.Id == id).Select(c => new
            {
                c.Id,
                c.Key,
                c.Name,
                c.Description,
                SelfUrl = $"/data/calendar/{c.Id}",
                Events = c.Events.Where(e => e.StartDate >= start && e.EndDate <= end).Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Description,
                    e.StartDate,
                    e.EndDate,
                    SelfUrl = $"/data/calendar/event/{e.Id}"
                })
            }).FirstOrDefault();
            return calendar != null ? Ok(calendar) : (IActionResult)NoContent();
        }
        #endregion
    }
}
