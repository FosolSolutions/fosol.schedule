﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// <typeparamref name="Activity"/> class, provides a way to manage activities within the datasource.  An event can have zero-or-more activities.
    /// </summary>
    public class Activity : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the parent event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// get/set - The parent event.
        /// </summary>
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this activity.
        /// </summary>
        [Required]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique name within an event to identify this activity.
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of this activity.
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// get/set - When the activity starts.
        /// </summary>
        public DateTime? StartOn { get; set; }

        /// <summary>
        /// get/set - When the activity ends.
        /// </summary>
        public DateTime? EndOn { get; set; }

        /// <summary>
        /// get/set - The state of the activity.
        /// </summary>
        public ActivityState State { get; set; } = ActivityState.Published;

        /// <summary>
        /// get/set - The order the activities will be displayed.
        /// </summary>
        [DefaultValue(0)]
        public int Sequence { get; set; }

        /// <summary>
        /// get/set - A collection of openings within this activity.
        /// </summary>
        public ICollection<Opening> Openings { get; private set; } = new List<Opening>();

        /// <summary>
        /// get/set - A collection of criteria within this activity.
        /// </summary>
        public ICollection<ActivityCriteria> Criteria { get; private set; } = new List<ActivityCriteria>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a <typeparamref name="Activity"/> object.
        /// </summary>
        public Activity()
        {

        }

        /// <summary>
        /// Creates a new instance of a <typeparamref name="Activity"/> object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="cevent"></param>
        /// <param name="name"></param>
        /// <param name="state"></param>
        public Activity(Event cevent, string name, ActivityState state = ActivityState.Published)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.EventId = cevent.Id;
            this.Event = cevent;
            this.Name = name;
            this.Key = Guid.NewGuid();
        }


        /// <summary>
        /// Creates a new instance of a <typeparamref name="Activity"/> object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="cevent"></param>
        /// <param name="name"></param>
        /// <param name="start">When the activity starts.</param>
        /// <param name="end">When the activity ends.</param>
        /// <param name="state"></param>
        public Activity(Event cevent, string name, DateTime start, DateTime? end = null, ActivityState state = ActivityState.Published) : this(cevent, name, state)
        {
            this.StartOn = start;
            this.EndOn = end;
        }
        #endregion
    }
}
