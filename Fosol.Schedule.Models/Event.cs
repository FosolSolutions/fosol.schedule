using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models
{
    public class Event : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        public int CalendarId { get; set; }

        public Guid Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }

        public IEnumerable<Activity> Activities { get; set; }

        public IEnumerable<Criteria> Criteria { get; set; }
        #endregion
    }
}
