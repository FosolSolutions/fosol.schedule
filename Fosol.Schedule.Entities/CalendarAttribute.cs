using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// CalendarAttribute class, provides a way to manage the many-to-many relationship between calendars and attributes.
    /// </summary>
    public class CalendarAttribute
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the calendar.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar associated with the attribute.
        /// </summary>
        [ForeignKey(nameof(CalendarId))]
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - Foreign key to the attribute.
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// get/set - The attribute associated with the calendar.
        /// </summary>
        [ForeignKey(nameof(AttributeId))]
        public Attribute Attribute { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CalendarAttribute object.
        /// </summary>
        public CalendarAttribute()
        {

        }

        /// <summary>
        /// Creates a new instance of a CalendarAttribute object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="attribute"></param>
        public CalendarAttribute(Calendar calendar, Attribute attribute)
        {
            this.CalendarId = calendar?.Id ?? throw new ArgumentNullException(nameof(calendar));
            this.AttributeId = attribute?.Id ?? throw new ArgumentNullException(nameof(attribute));
        }
        #endregion
    }
}
