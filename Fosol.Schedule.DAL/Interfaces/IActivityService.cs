﻿using System;
using System.Collections.Generic;

namespace Fosol.Schedule.DAL.Interfaces
{
  public interface IActivityService : IUpdatableService<Models.Create.Activity, Models.Read.Activity, Models.Update.Activity, Models.Delete.Activity>
  {
    /// <summary>
    /// Get the activity for the specified 'id'.
    /// Validates whether the current user is authorized to view the activity.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Models.Read.Activity Get(int id);

    /// <summary>
    /// Get all of the activities for the specified event.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="startOn"></param>
    /// <param name="endOn"></param>
    /// <returns></returns>
    IEnumerable<Models.Read.Activity> GetForEvent(int eventId, DateTime? startOn = null, DateTime? endOn = null);

    /// <summary>
    /// Get the activities for the specified 'calendard' and within the specified timeframe.
    /// Validates whether the current use is authorized to view the calendar.
    /// </summary>
    /// <param name="calendarId"></param>
    /// <param name="startOn"></param>
    /// <param name="endOn"></param>
    /// <returns></returns>
    IEnumerable<Models.Read.Activity> GetForCalendar(int calendarId, DateTime startOn, DateTime endOn);

  }
}