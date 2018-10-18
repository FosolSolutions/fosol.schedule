﻿using Fosol.Core.Mvc;
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
        /// <returns>An array calendar.</returns>
        [HttpGet("/[area]/calendars/{page}")]
        public IActionResult Calendars(int page)
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
        /// <returns>A calendar with all events within the specified date range.</returns>
        [HttpGet("{id}")]
        public IActionResult Calendar(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            // TODO: no tracking.
            var calendar = _dataSource.Calendars.Get(id, startOn, endOn);
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

            return Ok(calendar);
        }
        #endregion
    }
}
