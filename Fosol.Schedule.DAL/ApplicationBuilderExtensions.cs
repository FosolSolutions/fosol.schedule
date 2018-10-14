using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fosol.Schedule.DAL
{
    public static class ApplicationBuilderExtensions
    {
        #region Methods
        public static IApplicationBuilder UseDataSource(this IApplicationBuilder app, IHostingEnvironment env = null)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var datasource = scope.ServiceProvider.GetRequiredService<IDataSource>();
                //datasource.EnsureCreated();
                datasource.Migrate();
                //datasource.EnsureDeleted();
            }

            return app;
        }
        #endregion
    }
}
