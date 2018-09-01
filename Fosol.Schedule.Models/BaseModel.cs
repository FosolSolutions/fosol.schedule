using System;
using System.Text;

namespace Fosol.Schedule.Models
{
    public abstract class BaseModel
    {
        #region Properties
        public int AddedBy { get; set; }

        public DateTime AddedOn { get; set; } = DateTime.UtcNow;

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string RowVersion { get; set; }
        #endregion

        #region Constructors
        public BaseModel()
        {

        }

        public BaseModel(Entities.BaseEntity entity)
        {
            this.AddedBy = entity.AddedBy;
            this.AddedOn = entity.AddedOn;
            this.UpdatedBy = entity.UpdatedBy;
            this.UpdatedOn = entity.UpdatedOn;
            this.RowVersion = Encoding.UTF8.GetString(entity.RowVersion);
        }
        #endregion
    }
}