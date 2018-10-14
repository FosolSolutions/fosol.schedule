using Fosol.Schedule.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fosol.Schedule.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataSource(this IServiceCollection service, Action<DbContextOptionsBuilder> optionsBuilder)
        {
            var options = new DbContextOptions<ScheduleContext>();
            var builder = new DbContextOptionsBuilder<ScheduleContext>(options);
            optionsBuilder(builder);
            //service.AddDbContextPool<ScheduleContext>(optionsBuilder);
            var datasource = new DataSource(builder.Options);
            service.AddScoped<IDataSource>(provider => datasource);
            return service;
        }
    }
}
