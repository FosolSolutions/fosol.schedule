using System;

namespace Fosol.Schedule.Entities
{
    public abstract class BaseEntity
    {
        #region Properties
        public int AddedBy { get; set; }

        public DateTime AddedOn { get; set; } = DateTime.UtcNow;

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public byte[] RowVersion { get; set; }
        #endregion
    }
}