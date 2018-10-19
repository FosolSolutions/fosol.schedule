namespace Fosol.Schedule.DAL.Interfaces
{
    public interface IOpeningService : IUpdatableService<Models.Opening>
    {
        /// <summary>
        /// Get the opening for the specified 'id'.
        /// Validates whether the current user is authorized to view the opening.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Opening Get(int id);
    }
}