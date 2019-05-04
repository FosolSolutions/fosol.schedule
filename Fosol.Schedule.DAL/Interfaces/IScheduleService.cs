using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fosol.Schedule.DAL.Interfaces
{
	public interface IScheduleService : IUpdatableService<Models.Create.Schedule, Models.Read.Schedule, Models.Update.Schedule, Models.Delete.Schedule>
	{
		/// <summary>
		/// Get the schedule for the specified 'id'.
		/// Validates whether the current user is authorized to view the schedule.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Models.Update.Schedule Get(int id);

		/// <summary>
		/// Get the schedule for the specified 'id'.
		/// Validates whether the current user is authorized to view the schedule.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="startOn"></param>
		/// <param name="endOn"></param>
		/// <returns></returns>
		Models.Update.Schedule Get(int id, DateTime? startOn = null, DateTime? endOn = null);


		/// <summary>
		/// Add the events to the specified schedule.
		/// Checks to ensure duplicate events are not added to the same schedule.
		/// </summary>
		/// <param name="scheduleId"></param>
		/// <param name="events"></param>
		/// <returns></returns>
		IEnumerable<ValidationResult> AddEventsToSchedule(int scheduleId, IEnumerable<Models.Read.Event> events);
	}
}