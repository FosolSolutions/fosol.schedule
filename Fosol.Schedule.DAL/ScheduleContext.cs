using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// ScheduleContext private sealed class, provides a way connect to the datasource.
    /// </summary>
    sealed class ScheduleContext : DbContext
    {
        #region Variables
        private readonly string _connectionString;
        #endregion

        #region Properties
        public DbSet<Calendar> Calendars { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Activity> Activities { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ScheduleContext object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="connectionString"></param>
        public ScheduleContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Configures the DbContext with the specified options.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }
        #endregion
    }
}
