using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// An event is a specific time slot in a calendar.
    /// </summary>
    public class CalendarEvent
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key.  Unique way to identify the event.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the calendar.  All events are owned by a calendar.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - A name to identify the event.
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the event.
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// get/set - An event always has a start date and time.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// get/set - An event always has an end date and time.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// get/set - A collection of criteria which are required to participate in the events.
        /// </summary>
        public ICollection<Criteria> Criterias { get; set; }

        /// <summary>
        /// get/set - A collection of activities within this event.
        /// </summary>
        public ICollection<Activity> Activities { get; set; }
        #endregion
    }
}
