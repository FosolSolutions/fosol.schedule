using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// <typeparamref name="DataContext"/> private sealed class, provides a way connect to the datasource.
    /// </summary>
    sealed class ScheduleContext : DbContext
    {
        #region Variables
        private readonly string _connectionString;
        #endregion

        #region Properties
        public DbSet<Calendar> Calendars { get; set; }

        public DbSet<CalendarEvent> Events { get; set; }

        public DbSet<Activity> Activities { get; set; }
        #endregion

        #region Constructors
        public ScheduleContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }
        #endregion
    }
}
