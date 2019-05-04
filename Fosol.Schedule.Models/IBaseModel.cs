using System;

namespace Fosol.Schedule.Models
{
  public interface IBaseModel
  {
    /// <summary>
    /// get/set - Foreign key identifying which user created this entity.
    /// </summary>
    int? AddedById { get; set; }

    /// <summary>
    /// get/set - When this entity was created.
    /// </summary>
    DateTime? AddedOn { get; set; }

    /// <summary>
    /// get/set - Foreign key identifying which user updated this entity last.
    /// </summary>
    int? UpdatedById { get; set; }

    /// <summary>
    /// get/set - When this entity was updated last.
    /// </summary>
    DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// get/set - Timestamp identifying this particular entity state.  Used for concurrency.
    /// </summary>
    string RowVersion { get; set; }
  }
}