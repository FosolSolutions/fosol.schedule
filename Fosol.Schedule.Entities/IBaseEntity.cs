using System;

namespace Fosol.Schedule.Entities
{
  public interface IBaseEntity
  {
    /// <summary>
    /// get/set - Foreignkey to the user account who created this entity.
    /// </summary>
    int AddedById { get; set; }

    /// <summary>
    /// get/set - The user who added this record.
    /// </summary>
    User AddedBy { get; set; }

    /// <summary>
    /// get/set - When this entity was created.
    /// </summary>
    DateTime AddedOn { get; set; }

    /// <summary>
    /// get/set - Foreignkey to the user account who updated this entity last.
    /// </summary>
    int? UpdatedById { get; set; }

    /// <summary>
    /// get/set - The user who last updated this record.
    /// </summary>
    User UpdatedBy { get; set; }

    /// <summary>
    /// get/set - When this entity was updated last.
    /// </summary>
    DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// get/set - The timestamp that identifies the current state of this entity.  Used for concurrency.
    /// </summary>
    byte[] RowVersion { get; set; }
  }
}