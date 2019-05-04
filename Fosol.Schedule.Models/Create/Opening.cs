using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models.Create
{
  public class Opening : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - The foreign key to the parent activity.
    /// </summary>
    public int ActivityId { get; set; }

    /// <summary>
    /// get/set - A unique key to identify this opening.
    /// </summary>
    public Guid Key { get; set; }

    /// <summary>
    /// get/set - A name to identify this opening.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// get/set - A description about this opening.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// get/set - The minimum number of participants required for this opening.
    /// </summary>
    public int MinParticipants { get; set; }

    /// <summary>
    /// get/set - The maximum number of participants allowed in this opening.
    /// </summary>
    public int MaxParticipants { get; set; }

    /// <summary>
    /// get/set - The type of opening.
    /// </summary>
    public OpeningType OpeningType { get; set; }

    /// <summary>
    /// get/set - The process participants can apply for this opening.
    /// </summary>
    public ApplicationProcess ApplicationProcess { get; set; }

    /// <summary>
    /// get/set - The state of the opening.
    /// </summary>
    public OpeningState State { get; set; } = OpeningState.Published;

    /// <summary>
    /// get/set - How criteria is applied to participants in this opening.  This can be overridden in child entities.
    /// </summary>
    public CriteriaRule CriteriaRule { get; set; } = CriteriaRule.Participate;

    /// <summary>
    /// get/set - A collection of criteria for this opening.
    /// </summary>
    public IList<Criteria> Criteria { get; set; }

    /// <summary>
    /// get - A collection of tags for this opening.
    /// </summary>
    public IList<Tag> Tags { get; set; }

    /// <summary>
    /// get - A collection of questions asked when a participant applies for this opening.
    /// </summary>
    public IList<Question> Questions { get; set; }
    #endregion
  }
}