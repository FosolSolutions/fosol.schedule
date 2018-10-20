using Fosol.Core.Exceptions;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// ActivityService sealed class, provides a way to manage activities in the datasource.
    /// </summary>
    public sealed class ActivityService : UpdatableService<Activity, Models.Activity>, IActivityService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ActivityService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal ActivityService(IDataSource source) : base(source)
        {
            //Authenticated();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the activity for the specified 'id'.
        /// Validates whether the current user is authorized to view the activity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Activity Get(int id)
        {
            var calendarId = this.GetCalendarId();
            return this.Map(this.Find((set) => set.Include(a => a.Criteria).Include(a => a.Openings).ThenInclude(o => o.Criteria).SingleOrDefault(a => a.Id == id && a.Event.CalendarId == calendarId)));
        }

        /// <summary>
        /// Get the activities for the specified 'eventId' and within the specified timeframe.
        /// Validates whether the current user is authorized to view the activity.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="startOn"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Models.Activity> Get(int eventId, DateTime? startOn = null, DateTime? endOn = null)
        {
            var calendarId = this.GetCalendarId();
            var cevent = this.Find<Event>((set) => set.SingleOrDefault(e => e.Id == eventId && e.CalendarId == calendarId));

            // Convert datetime to utc.
            var start = startOn?.ToUniversalTime() ?? cevent.StartOn;
            var end = endOn?.ToUniversalTime() ?? cevent.EndOn;

            var activities = this.Context.Activities.Include(a => a.Criteria).Include(a => a.Openings).ThenInclude(o => o.Criteria).Where(a => a.EventId == eventId && a.StartOn >= start && a.EndOn <= end).OrderBy(a => a.StartOn).ThenBy(a => a.Sequence).ToArray().Select(a => this.Map(a));
            return activities;
        }
        #endregion
    }
}
