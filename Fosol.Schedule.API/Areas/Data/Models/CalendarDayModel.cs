using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Areas.Data.Models
{
    public class CalendarDayModel
    {
        #region Properties
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<CalendarEventModel> Events { get; set; }
        #endregion
    }
}
