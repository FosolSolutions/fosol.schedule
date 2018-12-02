using AutoMapper;
using Fosol.Schedule.DAL.Extensions;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.DAL.Maps;
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
			var profile = new ModelProfile();
			var mapper = new MapperConfiguration(config =>
			{
				//profile.BindDataSource(this);
				config.AllowNullCollections = true;
				config.AddProfile((Profile)profile);
			}).CreateMapper();
			services.AddSingleton<ModelProfile>(profile);
			services.AddSingleton<IMapper>(mapper);
			services.AddScoped<IDataSource, DataSource>();
			return services;
		}

		/// <summary>
		/// Adds the datasource to the service collection and configures the DbContext options within a DbContextPool.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="setupAction"></param>
		/// <returns></returns>
		public static IServiceCollection AddDataSourcePool(this IServiceCollection services, Action<DataSourceOptionsBuilder> setupAction)
		{
			services.AddSingleton<ModelProfile>();
			services.AddScoped<IDataSource, DataSource>();
			services.AddDbContextPool<ScheduleContext>((dbBuilder) =>
			{
				var builder = new DataSourceOptionsBuilder(dbBuilder);
				setupAction?.Invoke(builder);
				services.AddSingleton<DbContextOptions>(builder.Options);
				services.AddSingleton(builder.Options);
			});

			return services;
		}
	}
}
