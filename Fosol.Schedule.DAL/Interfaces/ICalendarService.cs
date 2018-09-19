using System.Collections.Generic;

namespace Fosol.Schedule.DAL.Interfaces
{
    public interface ICalendarService : IUpdatableService<Models.Calendar>
    {
        /// <summary>
        /// Get all of the models for the current user.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Models.Calendar> Get();
    }
}