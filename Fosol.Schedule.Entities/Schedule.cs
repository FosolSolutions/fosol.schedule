using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// A schedule provides a way to filter a collection of events from multiple calendars.
    /// </summary>
    public class Schedule : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this schedule.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A name to identify the schedule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the schedule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The start date of the schedule.
        /// </summary>
        public DateTime StartOn { get; set; }

        /// <summary>
        /// get/set - The end date of the schedule.
        /// </summary>
        public DateTime EndOn { get; set; }

        /// <summary>
        /// get/set - The current state of the schedule.
        /// </summary>
        public ScheduleState State { get; set; }

        /// <summary>
        /// get - A collection of events associated with this schedule.
        /// </summary>
        public ICollection<ScheduleEvent> Events { get; private set; } = new List<ScheduleEvent>(); // TODO: Must use many-to-many table because events are linked to Calendar and not Schedule.  Or I need to instead create a filter which will pull in events from calendars.
        #endregion
    }
}
