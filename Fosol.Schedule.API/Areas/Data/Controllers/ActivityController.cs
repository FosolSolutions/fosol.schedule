﻿using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
	/// <summary>
	/// ActivityController sealed class, provides API endpoints for calendar event activities.
	/// </summary>
	[Produces("application/json")]
	[Area("data")]
	[Route("[area]/calendar/event/[controller]")]
	[Authorize]
	public sealed class ActivityController : ApiController
	{
		#region Variables
		private readonly IDataSource _dataSource;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of a ActivityController object.
		/// </summary>
		/// <param name="datasource"></param>
		public ActivityController(IDataSource datasource)
		{
			_dataSource = datasource;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Returns an event activity for the specified 'id'.
		/// </summary>
		/// <param name="id">The primary key for the activity.</param>
		/// <returns>The activity JSON data object from the datasource.</returns>
		[HttpGet("{id}", Name = "GetActivity")]
		public IActionResult GetActivity(int id)
		{
			var activity = _dataSource.Activities.Get(id);
			return Ok(activity);
		}

		/// <summary>
		/// Returns an array of activities for the calendar specified by the 'id'.
		/// </summary>
		/// <param name="id">The calendar id.</param>
		/// <param name="startOn">The start date for the calendar to return.  Defaults to now.</param>
		/// <param name="endOn">The end date for the calendar to return.</param>
		/// <returns>An array of activities.</returns>
		[HttpGet("/[area]/calendar/{id}/activities")]
		public IActionResult GetActiviesForCalendar(int id, DateTime? startOn = null, DateTime? endOn = null)
		{
			var start = startOn ?? DateTime.UtcNow;
			// Start at the beginning of the week.
			start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
			var end = endOn ?? start.AddDays(7);

			var activities = _dataSource.Activities.GetForCalendar(id, start, end);
			return Ok(activities);
		}
		#endregion
	}
}
