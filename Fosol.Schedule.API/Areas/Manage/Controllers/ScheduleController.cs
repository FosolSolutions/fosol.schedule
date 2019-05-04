using Fosol.Core.Mvc;
using Fosol.Core.Mvc.Filters;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Fosol.Schedule.API.Areas.Manage.Controllers
{
	/// <summary>
	/// ScheduleController class, provides API endpoints for schedules.
	/// </summary>
	[Produces("application/json")]
	[Area("manage")]
	[Route("[area]/[controller]")]
	[Authorize]
	[ValidateModelFilter]
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
		[HttpGet("{id}", Name = nameof(GetSchedule))]
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

		/// <summary>
		/// Returns the specified schedule.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}", Name = nameof(GetSchedule))]
		public IActionResult GetSchedule(int id)
		{
			var schedule = _dataSource.Schedules.Get(id);
			return Ok(schedule);
		}

		/// <summary>
		/// Add the specified schedule to the datasource.
		/// </summary>
		/// <param name="schedule"></param>
		/// <returns></returns>
		[HttpPost("/[area]/[controller]")]
		public IActionResult AddSchedule([FromBody] Models.Create.Schedule schedule)
		{
			var result = _dataSource.Schedules.Add(schedule);
			_dataSource.CommitTransaction();

			return Created(Url.RouteUrl(nameof(GetSchedule), new { result.Id }), result);
		}

		/// <summary>
		/// Update the specified schedule in the datasource.
		/// </summary>
		/// <param name="schedule"></param>
		/// <returns></returns>
		[HttpPut("/[area]/[controller]")]
		public IActionResult UpdateSchedule([FromBody] Models.Update.Schedule schedule)
		{
			var result = _dataSource.Schedules.Update(schedule);
			_dataSource.CommitTransaction();

			return Ok(result);
		}

		/// <summary>
		/// Delete the specified schedule from the datasource.
		/// </summary>
		/// <param name="schedule"></param>
		/// <returns></returns>
		[HttpDelete("/[area]/[controller]")]
		public IActionResult DeleteSchedule([FromBody] Models.Delete.Schedule schedule)
		{
			_dataSource.Schedules.Remove(schedule);
			_dataSource.CommitTransaction();

			return Ok();
		}

		/// <summary>
		/// Add the events to the specified schedule.
		/// Only events within the schedule's date range will be accepted.  All others will return an error message response.
		/// </summary>
		/// <param name="scheduleId"></param>
		/// <param name="events"></param>
		/// <returns></returns>
		[HttpPost("/{scheduleId}/events")]
		public IActionResult AddEventsToSchedule(int scheduleId, [FromBody] Models.Read.Event[] events)
		{
			var schedule = _dataSource.Schedules.Get(scheduleId);

			var errors = _dataSource.Schedules.AddEventsToSchedule(scheduleId, events);
			_dataSource.Schedules.Update(schedule);
			_dataSource.CommitTransaction();

			if (errors.Count() > 0) return Ok(errors.Select(e => e.ErrorMessage));

			return Ok(new { EventsAdd = events.Count() });
		}
		#endregion
	}
}
