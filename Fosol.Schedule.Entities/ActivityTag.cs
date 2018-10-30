using System;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// ActivityTag class, provides a way to manage the many-to-many relationship between activitys and tags.
    /// </summary>
    public class ActivityTag
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the activity associated with the tag.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// get/set - The activity associated with the tag.
        /// </summary>
        public Activity Activity { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag key associated with the activity.
        /// </summary>
        public string TagKey { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag value associated with the activity.
        /// </summary>
        public string TagValue { get; set; }

        /// <summary>
        /// get/set - The tag associated with the activity.
        /// </summary>
        public Tag Tag { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a ActivityTag object.
        /// </summary>
        public ActivityTag()
        {

        }

        /// <summary>
        /// Creates a new instance of a ActivityTag object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarActivity"></param>
        /// <param name="tag"></param>
        public ActivityTag(Activity calendarActivity, Tag tag)
        {
            this.ActivityId = calendarActivity?.Id ?? throw new ArgumentNullException(nameof(calendarActivity));
            this.Activity = calendarActivity;
            this.TagKey = tag?.Key ?? throw new ArgumentNullException(nameof(tag));
            this.TagValue = tag?.Value ?? throw new ArgumentNullException(nameof(tag));
            this.Tag = tag;
        }
        #endregion
    }
}
