using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models.Create
{
  public class Schedule : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - A unique key to identify this schedule.
    /// </summary>
    public Guid Key { get; set; }

    /// <summary>
    /// get/set - A name to identify the schedule.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// get/set - A description of the schedule.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// get/set - The start date of the schedule.
    /// </summary>
    public DateTime StartOn { get; set; }

    /// <summary>
    /// get/set - The end date of the schedule.
    /// </summary>
    public DateTime EndOn { get; set; }

    /// <summary>
    /// get/set - The current state of the schedule.
    /// </summary>
    public ScheduleState State { get; set; } = ScheduleState.Published;
    #endregion
  }
}