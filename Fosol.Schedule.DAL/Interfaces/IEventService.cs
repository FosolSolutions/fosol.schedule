﻿using System;
using System.Collections.Generic;

namespace Fosol.Schedule.DAL.Interfaces
{
    public interface IEventService : IUpdatableService<Models.Event>
    {
        /// <summary>
        /// Get the event for the specified 'id'.
        /// Validates whether the current user is authorized to view the event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Event Get(int id);

        /// <summary>
        /// Get all of the events for the specified calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        IEnumerable<Models.Event> Get(int calendarId, DateTime startOn, DateTime endOn);
    }
}