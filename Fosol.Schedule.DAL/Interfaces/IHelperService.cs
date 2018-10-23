﻿using System;

namespace Fosol.Schedule.DAL.Interfaces
{
    public interface IHelperService
    {
        #region Methods
        /// <summary>
        /// Creates and adds a new ecclesial calendar with default events, activities, openings and criteria.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        Models.Calendar AddEcclesialEvents(int calendarId, DateTime? startOn = null, DateTime? endOn = null);
        #endregion
    }
}
