using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    public class Activity
    {
        #region Properties
        public int Id { get; set; }
        public int EventId { get; set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Opening> Openings { get; set; }
        public ICollection<Criteria> Criterias { get; set; }
        #endregion
    }
}
