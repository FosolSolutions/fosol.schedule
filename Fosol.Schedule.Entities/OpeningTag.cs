using System;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// OpeningTag class, provides a way to manage the many-to-many relationship between openings and tags.
    /// </summary>
    public class OpeningTag
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the opening associated with the tag.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The opening associated with the tag.
        /// </summary>
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag key associated with the opening.
        /// </summary>
        public string TagKey { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag value associated with the opening.
        /// </summary>
        public string TagValue { get; set; }

        /// <summary>
        /// get/set - The tag associated with the opening.
        /// </summary>
        public Tag Tag { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a OpeningTag object.
        /// </summary>
        public OpeningTag()
        {

        }

        /// <summary>
        /// Creates a new instance of a OpeningTag object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarOpening"></param>
        /// <param name="tag"></param>
        public OpeningTag(Opening calendarOpening, Tag tag)
        {
            this.OpeningId = calendarOpening?.Id ?? throw new ArgumentNullException(nameof(calendarOpening));
            this.Opening = calendarOpening;
            this.TagKey = tag?.Key ?? throw new ArgumentNullException(nameof(tag));
            this.TagValue = tag?.Value ?? throw new ArgumentNullException(nameof(tag));
            this.Tag = tag;
        }
        #endregion
    }
}
