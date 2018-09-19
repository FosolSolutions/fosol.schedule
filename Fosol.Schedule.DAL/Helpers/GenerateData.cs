using Fosol.Schedule.Entities;
using System;
using System.Collections.Generic;

namespace Fosol.Schedule.DAL.Helpers
{
    static class GenerateData
    {
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

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
                    Name = "Memorial Meeting",
                    Description = "Sunday memorial meeting.",
                    StartOn = sunday.AddHours(11),
                    EndOn = sunday.AddHours(13),
                    Id = i++
                });
                sunday = sunday.AddDays(7);
            }

            return calendar;
        }
        #endregion
    }
}
