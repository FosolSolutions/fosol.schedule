using System;

namespace Fosol.Schedule.DAL.Interfaces
{
    public interface IParticipantService : IUpdatableService<Models.Participant>
    {
        /// <summary>
        /// Get the calendar for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Participant Get(int id);

        /// <summary>
        /// Get the calendar for the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Models.Participant Get(Guid key);
    }
}