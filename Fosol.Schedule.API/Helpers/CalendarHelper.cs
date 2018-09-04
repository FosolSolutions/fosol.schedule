using Fosol.Schedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Schedule.API.Helpers
{
    /// <summary>
    /// CalendarHelper static class, provides helper functions to create fake calendars.
    /// </summary>
    public static class CalendarHelper
    {
        #region Methods
        /// <summary>
        /// Creates a collection of fake calendars.
        /// </summary>
        /// <returns>A new List of Calendar object.</returns>
        public static List<Calendar> CreateCalendars()
        {
            return new List<Calendar>()
            {
                CreateCalendar(1),
                CreateCalendar(2)
            };
        }

        /// <summary>
        /// Creates a fake calendar with events.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A new Calendar object.</returns>
        public static Calendar CreateCalendar(int id)
        {
            var events = new List<Event>(365);
            var calendar = new Calendar()
            {
                Id = id,
                Key = Guid.NewGuid(),
                Name = $"calendar {id}",
                Description = $"calendar {id}",
                Events = events
            };

            // Sunday Memorial
            var jan1st = new DateTime(DateTime.Now.Year, 1, 1);
            var sunday = jan1st.DayOfWeek == DayOfWeek.Sunday ? jan1st : jan1st.AddDays(7 - (int)jan1st.DayOfWeek);
            var i = 1;
            while (sunday.Year == DateTime.Now.Year)
            {
                events.Add(new Event()
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
