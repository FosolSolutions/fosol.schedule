﻿using System;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// <typeparamref name="BaseEntity"/> abstract class, provides a way to ensure all entities have commonly shared properties.
    /// </summary>
    public abstract class BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Foreignkey to the user account who created this entity.
        /// </summary>
        public int? AddedById { get; set; }

        /// <summary>
        /// get/set - The user who added this record.
        /// </summary>
        public User AddedBy { get; set; }

        /// <summary>
        /// get/set - When this entity was created.
        /// </summary>
        public DateTime AddedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// get/set - Foreignkey to the user account who updated this entity last.
        /// </summary>
        public int? UpdatedById { get; set; }

        /// <summary>
        /// get/set - The user who last updated this record.
        /// </summary>
        public User UpdatedBy { get; set; }

        /// <summary>
        /// get/set - When this entity was updated last.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// get/set - The timestamp that identifies the current state of this entity.  Used for concurrency.
        /// </summary>
        public byte[] RowVersion { get; set; }
        #endregion
    }
}