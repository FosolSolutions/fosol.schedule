using System.ComponentModel.DataAnnotations;

namespace Fosol.Schedule.Entities
{
    public class Quality
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to calendar.
        ///     Qualities are calendar specified, but a user may have global qualities that apply to all calendars (i.e. gender, birthdate)
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