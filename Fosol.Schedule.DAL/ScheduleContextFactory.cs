using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Fosol.Schedule.DAL
{
    class ScheduleContextFactory : IDesignTimeDbContextFactory<ScheduleContext>
    {
        #region Constructors
        public ScheduleContextFactory()
        {

        }
        #endregion

        #region Methods
        public ScheduleContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = Directory.GetCurrentDirectory();
            if (args.Length > 0) environmentName = args[0];

            return CreateDbContext(basePath, environmentName);
        }

        private ScheduleContext CreateDbContext(string basePath, string environmentName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("connectionstrings.json")
                .AddJsonFile($"connectionStrings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("coevent");

            if (String.IsNullOrWhiteSpace(connectionString) == true)
            {
                throw new InvalidOperationException("Could not find a connection string named '(coevent)'.");
            }
            else
            {
                return Create(connectionString);
            }
        }

        private ScheduleContext Create(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<ScheduleContext>();
            builder.UseSqlServer(connectionString);

            return new ScheduleContext(builder.Options);
        }
        #endregion
    }
}
