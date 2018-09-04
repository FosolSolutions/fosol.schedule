using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// <typeparamref name="Event"/> class, provides a way to manage events in the datasource.  An event is a specific time slot in a calendar.
    /// </summary>
    public class Event : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.  Unique way to identify the event.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the calendar.  All events are owned by a calendar.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar this event belongs to.
        /// </summary>
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this event.
        /// </summary>
        public Guid Key { get; set; }

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
        /// get - A collection of criteria which are required to participate in the events.
        /// </summary>
        public ICollection<Criteria> Criteria { get; set; }

        /// <summary>
        /// get - A collection of activities within this event.
        /// </summary>
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        /// <summary>
        /// get - A collection of tags associated with this event.
        /// </summary>
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a <typeparamref name="Event"/> object.
        /// </summary>
        public Event()
        {

        }

        /// <summary>
        /// Creates a new instance of a <typeparamref name="Event"/> object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Event(int calendarId, string name, DateTime start, DateTime end)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.CalendarId = calendarId;
            this.Name = name;
            this.Key = Guid.NewGuid();
            this.StartDate = start;
            this.EndDate = end;
        }
        #endregion
    }
}
