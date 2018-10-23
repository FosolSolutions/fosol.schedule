using Fosol.Schedule.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fosol.Schedule.DAL
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the datasource to the service collection and configures the DbContext options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataSource(this IServiceCollection services, Action<DbContextOptionsBuilder> setupAction)
        {
            var builder = new DbContextOptionsBuilder<ScheduleContext>();
            setupAction?.Invoke(builder);
            services.AddSingleton<DbContextOptions>(builder.Options);
            services.AddSingleton(builder.Options);
            services.AddScoped<IDataSource, DataSource>();
            return services;
        }
    }
}
