using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models
{
    public class Schedule : BaseModel
    {
        #region Properties
        public int Id { get; set; }
        public Guid Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// get/set - The foreign key to the user who owns this schedule.
        /// </summary>
        public int OwnerId { get; set; }
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }

        public IList<Event> Events { get; set; }
        #endregion
    }
}
