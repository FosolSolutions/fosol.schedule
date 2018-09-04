using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// CalendarService sealed class, provides a way to manage calendars in the datasource.
    /// </summary>
    public sealed class CalendarService : UpdatableService<Entities.Calendar, Models.Calendar>, ICalendarService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CalendarService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal CalendarService(DataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get all the calendars owned by the current user.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Calendar> Get()
        {
            return this.Source.Context.Calendars.Select(c => this.Source.Mapper.Map<Entities.Calendar, Models.Calendar>(c));
        }
        #endregion
    }
}
