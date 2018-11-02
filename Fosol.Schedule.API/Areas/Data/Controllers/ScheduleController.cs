using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// ScheduleController class, provides API endpoints for schedules.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/[controller]")]
    public sealed class ScheduleController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ScheduleController object.
        /// </summary>
        /// <param name="dataSource"></param>
        public ScheduleController(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the specified schedule and its events for the current week (or timespan).
        /// </summary>
        /// <param name="id">The primary key to identify the schedule.</param>
        /// <param name="startOn">The start date for the schedule to return.  Defaults to now.</param>
        /// <param name="endOn">The end date for the schedule to return.</param>
        /// <returns>A schedule JSON data object with all events within the specified date range.</returns>
        [HttpGet("{id}")]
        public IActionResult GetSchedule(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            // TODO: no tracking.
            var schedule = _dataSource.Schedules.Get(id, start, end);
            return schedule != null ? Ok(schedule) : (IActionResult)NoContent();
        }
        #endregion
    }
}
