using System;
using System.Collections.Generic;

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

        #region Methods
        /// <summary>
        /// Convert the entity into a model.
        /// </summary>
        /// <param name="calendar"></param>
        public static explicit operator Calendar(Entities.Calendar calendar)
        {
            return new Calendar()
            {
                Id = calendar.Id,
                Key = calendar.Key,
                Name = calendar.Name,
                Description = calendar.Description,
                SelfUrl = $"/data/calendar/{calendar.Id}"
            };
        }
        #endregion
    }
}
