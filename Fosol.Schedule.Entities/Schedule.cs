﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// A schedule provides a way to filter a collection of events from multiple calendars.
    /// </summary>
    public class Schedule
    {
        #region Properties
        public int Id { get; set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Event> Events { get; set; } // TODO: Must use many-to-many table because events are linked to Calendar and not Schedule.  Or I need to instead create a filter which will pull in events from calendars.
        #endregion
    }
}
