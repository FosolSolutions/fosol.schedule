using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.DAL.Helpers
{
    static class ScheduleMapper
    {
        #region Properties
        public static RelationalMapper Map = new RelationalMapper<ScheduleContext>();
        #endregion
    }
}
