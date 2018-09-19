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
        /// get - The service to manage calendars.
        /// </summary>
        ICalendarService Calendars { get; }
        #endregion

        #region Methods
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
