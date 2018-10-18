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
        /// Creates a calendar with events for Victoria Ecclesia.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A new Calendar object.</returns>
        public static Calendar CreateEcclesialCalendar(ScheduleContext context)
        {
            var account = context.Accounts.Find(1);
            var events = new List<Event>(365);
            var calendar = new Calendar(account, "Victoria Christadelphian Ecclesia")
            {
                Description = "The Victoria Christadelphian ecclesial calendar"
            };

            // Sunday Memorial
            var jan1st = new DateTime(DateTime.Now.Year, 1, 1);
            var sunday = jan1st.DayOfWeek == DayOfWeek.Sunday ? jan1st : jan1st.AddDays(7 - (int)jan1st.DayOfWeek);
            while (sunday.Year == DateTime.Now.Year)
            {
                calendar.Events.Add(new Event(calendar, "Memorial Meeting", sunday.AddHours(11), sunday.AddHours(13))
                {
                    Description = "Sunday memorial meeting."
                });
                sunday = sunday.AddDays(7);
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Calendars.Add(calendar);
                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return calendar;
        }
        #endregion
    }
}
