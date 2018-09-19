using System.ComponentModel.DataAnnotations;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Attribute class, provides a way to describe an attribute or qualification that a participant has.
    /// </summary>
    public class Attribute
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to calendar.
        ///     Attributes are calendar specified, but a user may have global attributes that apply to all calendars (i.e. gender, birthdate)
        /// </summary>
        public int? CalendarId { get; set; }

        /// <summary>
        /// get/set - Primary key.  A unique way to identify a quality.
        /// </summary>
        [Required, MaxLength(100)]
        public string Key { get; set; }

        /// <summary>
        /// get/set - Primary key.  A value to specify the quality.
        /// </summary>
        [Required, MaxLength(100)]
        public string Value { get; set; }
        
        /// <summary>
        /// get/set - The datatype of the quality.
        /// </summary>
        public DataType DataType { get; set; }
        #endregion
    }
}