using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Fosol.Schedule.DAL.Extensions
{
    public class DataSourceOptionsBuilder
    {
        #region Properties
        internal DbContextOptions<ScheduleContext> Options { get { return (DbContextOptions<ScheduleContext>)this.DbContextOptionsBuilder.Options; } }

        public DbContextOptionsBuilder DbContextOptionsBuilder { get; private set; }
        #endregion

        #region Constructors
        public DataSourceOptionsBuilder()
        {
            this.DbContextOptionsBuilder = new DbContextOptionsBuilder(new DbContextOptions<ScheduleContext>());
        }

        public DataSourceOptionsBuilder(DbContextOptionsBuilder dbOptionsBuilder)
        {
            this.DbContextOptionsBuilder = dbOptionsBuilder;
        }
        #endregion

        #region Methods
        public void Build(DbContextOptionsBuilder builder)
        {

        }

        public DataSourceOptionsBuilder UseApplicationServiceProvider(IServiceProvider serviceProvider)
        {
            this.DbContextOptionsBuilder.UseApplicationServiceProvider(serviceProvider);

            return this;
        }

        public DataSourceOptionsBuilder UseLoggerFactory(ILoggerFactory loggerFactory)
        {
            this.DbContextOptionsBuilder.UseLoggerFactory(loggerFactory);

            return this;
        }

        public DataSourceOptionsBuilder UseSqlServer(string connectionString)
        {
            this.DbContextOptionsBuilder.UseSqlServer(connectionString);

            return this;
        }

        public DataSourceOptionsBuilder UseSqlServer(DbConnection connection)
        {
            this.DbContextOptionsBuilder.UseSqlServer(connection);

            return this;
        }

        public DataSourceOptionsBuilder EnableSensitiveDataLogging()
        {
            this.DbContextOptionsBuilder.EnableSensitiveDataLogging();

            return this;
        }
        #endregion
    }
}
