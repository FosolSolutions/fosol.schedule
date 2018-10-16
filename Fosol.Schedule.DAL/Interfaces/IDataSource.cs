using AutoMapper;
using System;

namespace Fosol.Schedule.DAL.Interfaces
{
    /// <summary>
    /// IDataSource interface, provides a way to connect, query and manage data in the datasource.
    /// </summary>
    public interface IDataSource
    {
        #region Properties
        /// <summary>
        /// get - The AutoMapper used to cast objects.
        /// </summary>
        IMapper Mapper { get; }

        /// <summary>
        /// get - The service to manage users.
        /// </summary>
        IUserService Users { get; }

        /// <summary>
        /// get - The service to manage calendars.
        /// </summary>
        ICalendarService Calendars { get; }

        /// <summary>
        /// get - The service to manage participants.
        /// </summary>
        IParticipantService Participants { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Applies any pending migrations for the context to the database.  Will create the database if it does not already exist.
        /// </summary>
        void Migrate();

        /// <summary>
        /// Ensures that the database for the context exists.  If it exists, no action is taken.  IF it does not exist then the database adn all its schema are created.  If the database exists, then no effort is made to ensure it is compatible with the model for this context.
        /// </summary>
        /// <returns></returns>
        bool EnsureCreated();

        /// <summary>
        /// Ensures that the database for the context does not exist.  If it does not exist, no action is taken.  If it does exist then the database is deleted.
        /// </summary>
        /// <returns></returns>
        bool EnsureDeleted();

        /// <summary>
        /// Commit the changes in-memory to the datasource.
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// Commit the changes in-memory to the datasource in a single transaction.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        int CommitTransaction(Func<int> action);
        #endregion
    }
}
