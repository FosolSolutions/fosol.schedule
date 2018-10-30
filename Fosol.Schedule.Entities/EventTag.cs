using System;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// EventTag class, provides a way to manage the many-to-many relationship between events and tags.
    /// </summary>
    public class EventTag
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the event associated with the tag.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// get/set - The event associated with the tag.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag key associated with the event.
        /// </summary>
        public string TagKey { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag value associated with the event.
        /// </summary>
        public string TagValue { get; set; }

        /// <summary>
        /// get/set - The tag associated with the event.
        /// </summary>
        public Tag Tag { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a EventTag object.
        /// </summary>
        public EventTag()
        {

        }

        /// <summary>
        /// Creates a new instance of a EventTag object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <param name="tag"></param>
        public EventTag(Event calendarEvent, Tag tag)
        {
            this.EventId = calendarEvent?.Id ?? throw new ArgumentNullException(nameof(calendarEvent));
            this.Event = calendarEvent;
            this.TagKey = tag?.Key ?? throw new ArgumentNullException(nameof(tag));
            this.TagValue = tag?.Value ?? throw new ArgumentNullException(nameof(tag));
            this.Tag = tag;
        }
        #endregion
    }
}
