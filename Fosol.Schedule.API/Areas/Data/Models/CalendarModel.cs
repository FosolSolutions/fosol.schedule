using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Areas.Data.Models
{
    public class CalendarModel
    {
        #region Properties
        public int Id { get; set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<CalendarEventModel> Events { get; set; }
        #endregion
    }
}
