using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// DataSource sealed class, provides a way to interact with the datasource.
    /// </summary>
    public sealed class DataSource : IDataSource
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The DbContext used to communicate with the datasource.
        /// </summary>
        internal ScheduleContext Context { get; }

        /// <summary>
        /// get - The AutoMapper used to cast objects.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// get - The service to manage calendars.
        /// </summary>
        public ICalendarService Calendars { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DataSource object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="connectionString"></param>
        public DataSource(string connectionString)
        {
            this.Context = new ScheduleContext(connectionString);
            this.Mapper = new MapperConfiguration(config =>
            {
            }).CreateMapper();
            this.Calendars = new CalendarService(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Commit the in-memory changes to the datasource.
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return this.Context.SaveChanges();
        }

        /// <summary>
        /// Commit the in-memory changes to the datasource within an single transaction.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int CommitTransaction(Func<int> action)
        {
            int result;
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    result = action?.Invoke() ?? this.Context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return result;
        }
        #endregion
    }
}
