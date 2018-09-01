using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models
{
    public class CalendarEvent : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SelfUrl { get; set; }

        public string ParentUrl { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        #endregion
    }
}
