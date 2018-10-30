using System;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// CalendarTag class, provides a way to manage the many-to-many relationship between calendars and tags.
    /// </summary>
    public class CalendarTag
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the calendar associated with the tag.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar associated with the tag.
        /// </summary>
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag key associated with the calendar.
        /// </summary>
        public string TagKey { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag value associated with the calendar.
        /// </summary>
        public string TagValue { get; set; }

        /// <summary>
        /// get/set - The tag associated with the calendar.
        /// </summary>
        public Tag Tag { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a CalendarTag object.
        /// </summary>
        public CalendarTag()
        {

        }

        /// <summary>
        /// Creates a new instance of a CalendarTag object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarCalendar"></param>
        /// <param name="tag"></param>
        public CalendarTag(Calendar calendarCalendar, Tag tag)
        {
            this.CalendarId = calendarCalendar?.Id ?? throw new ArgumentNullException(nameof(calendarCalendar));
            this.Calendar = calendarCalendar;
            this.TagKey = tag?.Key ?? throw new ArgumentNullException(nameof(tag));
            this.TagValue = tag?.Value ?? throw new ArgumentNullException(nameof(tag));
            this.Tag = tag;
        }
        #endregion
    }
}
