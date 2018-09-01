using Fosol.Schedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// <typeparamref name="DataSource"/> sealed class, provides a way to interact with the datasource.
    /// </summary>
    public sealed class DataSource
    {
        #region Properties
        private readonly ScheduleContext _context;
        #endregion

        #region Constructors
        public DataSource(string connectionString)
        {
            _context = new ScheduleContext(connectionString);
        }
        #endregion

        #region Methods
        public IEnumerable<Calendar> GetCalendars()
        {
            return _context.Calendars.Select(c => new Calendar(c));
        }

        public Calendar GetCalendar(int id)
        {
            var calendar = _context.Calendars.Find(id);
            return new Calendar(calendar);
        }
        #endregion
    }
}
