using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// EventCriteria class, provides a way to manage the many-to-many relationship between events and criteria.
    /// </summary>
    public class EventCriteria
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// get/set - The event associated with the criteria.
        /// </summary>
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; }

        /// <summary>
        /// get/set - Foreign key to the criteria.
        /// </summary>
        public int CriteriaId { get; set; }

        /// <summary>
        /// get/set - The criteria associated with the event.
        /// </summary>
        [ForeignKey(nameof(CriteriaId))]
        public Criteria Criteria { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EventCriteria object.
        /// </summary>
        public EventCriteria()
        {

        }

        /// <summary>
        /// Creates a new instance of a EventCriteria object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <param name="criteria"></param>
        public EventCriteria(Event calendarEvent, Criteria criteria)
        {
            this.EventId = calendarEvent?.Id ?? throw new ArgumentNullException(nameof(calendarEvent));
            this.Event = calendarEvent;
            this.CriteriaId = criteria?.Id ?? throw new ArgumentNullException(nameof(criteria));
            this.Criteria = criteria;
        }
        #endregion
    }
}
