using System;

namespace Fosol.Schedule.DAL.Interfaces
{
    public interface IUserService : IUpdatableService<Models.User>
    {
        /// <summary>
        /// Get the calendar for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.User Get(int id);

        /// <summary>
        /// Get the calendar for the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Models.User Get(Guid key);
    }
}