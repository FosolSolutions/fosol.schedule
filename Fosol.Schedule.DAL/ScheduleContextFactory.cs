using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionstrings.json")
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<ScheduleContext>();

            var connectionString = configuration.GetConnectionString("coevent");

            builder.UseSqlServer(connectionString);

            return new ScheduleContext(builder.Options);
        }
        #endregion
    }
}
