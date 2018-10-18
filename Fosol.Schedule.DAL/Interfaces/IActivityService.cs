using System;
using System.Collections.Generic;

namespace Fosol.Schedule.DAL.Interfaces
{
    public interface IActivityService : IUpdatableService<Models.Activity>
    {
        /// <summary>
        /// Get the activity for the specified 'id'.
        /// Validates whether the current user is authorized to view the activity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Activity Get(int id);

        /// <summary>
        /// Get all of the activities for the specified event.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        IEnumerable<Models.Activity> Get(int eventId, DateTime? startOn = null, DateTime? endOn = null);
    }
}