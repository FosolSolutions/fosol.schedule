using Fosol.Schedule.DAL.Interfaces;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// OpeningService sealed class, provides a way to manage openings in the datasource.
    /// </summary>
    public sealed class OpeningService : UpdatableService<Entities.Opening, Models.Opening>, IOpeningService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal OpeningService(IDataSource source) : base(source)
        {
            //Authenticated();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the opening for the specified 'id'.
        /// Validates whether the current user is authorized to view the opening.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Opening Get(int id)
        {
            return this.Find(id);
        }
        #endregion
    }
}
