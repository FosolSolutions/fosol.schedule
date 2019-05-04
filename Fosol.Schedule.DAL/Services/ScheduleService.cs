using Fosol.Core.Extensions.Enumerable;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Fosol.Schedule.DAL.Services
{
	/// <summary>
	/// ScheduleService sealed class, provides a way to manage schedules in the datasource.
	/// </summary>
	public sealed class ScheduleService : UpdatableService<Entities.Schedule, Models.Create.Schedule, Models.Read.Schedule, Models.Update.Schedule, Models.Delete.Schedule>, IScheduleService
	{
		#region Variables
		#endregion

		#region Properties
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of a ScheduleService object, and initalizes it with the specified options.
		/// </summary>
		/// <param name="source"></param>
		internal ScheduleService(IDataSource source) : base(source)
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Get the schedule for the specified 'id'.
		/// Validates whether the current user is authorized to view the schedule.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Models.Update.Schedule Get(int id)
		{
			// TODO: Is user allowed ot see schedule?
			return this.GetWithTracking(e => e.Id == id);
		}

		/// <summary>
		/// Get the schedule for the specified 'id'.
		/// Validates whether the current user is authorized to view the schedule.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Models.Update.Schedule Get(int id, DateTime? startOn, DateTime? endOn)
		{
			var schedule = this.Get(id);

			// TODO: Add events.
			return schedule;
		}

		/// <summary>
		/// Add the events to the specified schedule.
		/// Checks to ensure duplicate events are not added to the same schedule.
		/// </summary>
		/// <param name="scheduleId"></param>
		/// <param name="events"></param>
		/// <returns></returns>
		public IEnumerable<ValidationResult> AddEventsToSchedule(int scheduleId, IEnumerable<Models.Read.Event> events)
		{
			var schedule = this.Find<Entities.Schedule>(scheduleId);

			// Get all the event Ids in the schedule to ensure they are not added again.
			var eventIds = this.Context.Schedules.Where(s => s.Id == scheduleId).SelectMany(s => s.Events.Select(e => e.EventId)).ToArray();

			var errors = new List<ValidationResult>();
			events.ForEach(e =>
			{
				if (e.StartOn < schedule.StartOn)
				{
					errors.Add(new ValidationResult($"Event [{e.Id}] \"{e.Name}\" occurs before the schedule and therefore will not be included.", new[] { nameof(schedule.StartOn) }));
				}
				else if (e.EndOn > schedule.EndOn)
				{
					errors.Add(new ValidationResult($"Event [{e.Id}] \"{e.Name}\" occurs after the schedule and therefore will not be included.", new[] { nameof(schedule.EndOn) }));
				}
				else if (eventIds.Contains(e.Id.Value))
				{
					errors.Add(new ValidationResult($"Event [{e.Id}] \"{e.Name}\" has already been included in the schedule."));
				}
				else
				{
					var cevent = this.Find<Entities.Event>(e.Id);
					var sevent = new Entities.ScheduleEvent(schedule, cevent);
					schedule.Events.Add(sevent);
				}
			});

			return errors.ToArray();
		}
		#endregion
	}
}
