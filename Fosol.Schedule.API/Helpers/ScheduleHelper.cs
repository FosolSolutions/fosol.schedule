using Fosol.Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Helpers
{
    public static class ScheduleHelper
    {
        #region Methods
        public static List<Calendar> CreateCalendars()
        {
            return new List<Calendar>()
            {
                CreateCalendar(1),
                CreateCalendar(2)
            };
        }

        public static Calendar CreateCalendar(int id)
        {
            var calendar = new Calendar()
            {
                Id = id,
                Key = Guid.NewGuid(),
                Name = $"calendar {id}",
                Description = $"calendar {id}",
                Events = new List<CalendarEvent>(365)
            };

            // Sunday Memorial
            var jan1st = new DateTime(DateTime.Now.Year, 1, 1);
            var sunday = jan1st.DayOfWeek == DayOfWeek.Sunday ? jan1st : jan1st.AddDays(7 - (int)jan1st.DayOfWeek);
            var i = 1;
            while (sunday.Year == DateTime.Now.Year)
            {
                calendar.Events.Add(new CalendarEvent()
                {
                    Id = i++,
                    Name = "Memorial Meeting",
                    Description = "Sunday memorial meeting.",
                    StartDate = sunday.AddHours(11),
                    EndDate = sunday.AddHours(13)
                });
                sunday = sunday.AddDays(7);
            }

            return calendar;
        }
        #endregion
    }
}
