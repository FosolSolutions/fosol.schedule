using Fosol.Overseer;
using Fosol.Schedule.DAL.Extensions;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fosol.Schedule.DAL
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the datasource to the service collection and configures the DbContext options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataSource(this IServiceCollection services, Action<DataSourceOptionsBuilder> setupAction)
        {
            var builder = new DataSourceOptionsBuilder();
            setupAction?.Invoke(builder);
            services.AddSingleton<DbContextOptions>(builder.Options);
            services.AddSingleton(builder.Options);
            services.AddScoped<IDataSource, DataSource>();
            services.AddOverseer();
            return services;
        }

        public static IServiceCollection AddDataSourcePool(this IServiceCollection services, Action<DataSourceOptionsBuilder> setupAction)
        {
            services.AddScoped<IDataSource, DataSource>();
            services.AddDbContextPool<ScheduleContext>((dbBuilder) =>
            {
                var builder = new DataSourceOptionsBuilder(dbBuilder);
                setupAction?.Invoke(builder);
                services.AddSingleton<DbContextOptions>(builder.Options);
                services.AddSingleton(builder.Options);
                services.AddOverseer();
            });

            return services;
        }
    }
}
