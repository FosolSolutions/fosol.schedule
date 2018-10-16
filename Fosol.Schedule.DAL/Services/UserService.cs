using Fosol.Core.Exceptions;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Linq;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// UserService sealed class, provides a way to manage users in the datasource.
    /// </summary>
    public sealed class UserService : UpdatableService<Entities.User, Models.User>, IUserService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal UserService(IDataSource source) : base(source)
        {
            //Authenticated();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the user for the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Models.User Get(Guid key)
        {
            var user = this.Context.Users.FirstOrDefault(p => p.Key == key) ?? throw new NoContentException(typeof(Models.User));

            return this.Source.Mapper.Map<Models.User>(user);
        }

        /// <summary>
        /// Get the user for the specified 'id'.
        /// Validates whether the current user is authorized to view the user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.User Get(int id)
        {
            var user = this.Find(id);

            return this.Source.Mapper.Map<Models.User>(user);
        }
        #endregion
    }
}
