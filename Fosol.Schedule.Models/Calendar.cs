using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models
{
    public class Calendar : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        public Guid Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SelfUrl { get; set; }

        public IEnumerable<CalendarEvent> Events { get; set; }
        #endregion

        #region Constructors
        public Calendar()
        {

        }

        public Calendar(Entities.Calendar calendar) : base(calendar)
        {
            if (calendar == null)
                throw new ArgumentNullException(nameof(calendar));

            this.Id = calendar.Id;
            this.Key = calendar.Key;
            this.Name = calendar.Name;
            this.Description = calendar.Description;
            this.SelfUrl = $"/data/calendar/{calendar.Id}";
        }
        #endregion

        #region Methods
        public static explicit operator Entities.Calendar(Calendar calendar)
        {
            return new Entities.Calendar()
            {
                Key = calendar.Key,
                Name = calendar.Name,
                Description = calendar.Description,
                AddedOn = calendar.AddedOn,
                AddedBy = calendar.AddedBy,
                UpdatedOn = calendar.UpdatedOn,
                UpdatedBy = calendar.UpdatedBy,
                RowVersion = Encoding.UTF8.GetBytes(calendar.RowVersion)
            };
        }
        #endregion
    }
}
