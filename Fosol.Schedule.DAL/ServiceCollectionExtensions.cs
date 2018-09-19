using Fosol.Schedule.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fosol.Schedule.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataSource(this IServiceCollection service, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Schedule") ?? @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0";
            var datasource = new DataSource(connectionString);
            service.AddScoped<IDataSource>((provider) => datasource);
            return service;
        }
    }
}
