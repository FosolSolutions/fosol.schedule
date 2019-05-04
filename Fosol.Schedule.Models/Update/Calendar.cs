using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models.Update
{
  public class Calendar : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - Primary key uses IDENTITY.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - A unique key to identify this calendar.
    /// </summary>
    public Guid Key { get; set; }

    /// <summary>
    /// get/set - A unique name within an account that identifies this calendar.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// get/set - A description of the calendar.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// get/set - The state of the calendar.
    /// </summary>
    public CalendarState State { get; set; } = CalendarState.Published;

    /// <summary>
    /// get/set - How criteria is applied to participants in this calendar.  This can be overridden in child entities.
    /// </summary>
    public CriteriaRule CriteriaRule { get; set; } = CriteriaRule.Participate;

    /// <summary>
    /// get - A collection of criteria for this calendar.
    /// </summary>
    public IList<Criteria> Criteria { get; private set; }

    /// <summary>
    /// get - A collection of tags for this calendar.
    /// </summary>
    public IList<Tag> Tags { get; private set; }

    /// <summary>
    /// get - A collection of attributes for this calendar.
    /// </summary>
    public IList<CalendarAttribute> Attributes { get; private set; }
    #endregion
  }
}