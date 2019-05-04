﻿using System;
using System.Text;

namespace Fosol.Schedule.Models
{
  /// <summary>
  /// <typeparamref name="BaseModel"/> abstract class, provdies a way to include default properties on all models.
  /// </summary>
  public abstract class BaseModel : IBaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - Foreign key identifying which user created this entity.
    /// </summary>
    public int? AddedById { get; set; }

    /// <summary>
    /// get/set - When this entity was created.
    /// </summary>
    public DateTime? AddedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// get/set - Foreign key identifying which user updated this entity last.
    /// </summary>
    public int? UpdatedById { get; set; }

    /// <summary>
    /// get/set - When this entity was updated last.
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// get/set - Timestamp identifying this particular entity state.  Used for concurrency.
    /// </summary>
    public string RowVersion { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a <typeparamref name="BaseModel"/> object.
    /// </summary>
    public BaseModel()
    {

    }
    #endregion
  }
}