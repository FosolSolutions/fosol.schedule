using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fosol.Schedule.API
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;
                    config.AddJsonFile("appSettings.json", false, true)
                    .AddJsonFile($"appSettings.{env.EnvironmentName}.json", true, true)
                    .AddJsonFile("connectionStrings.json", false, true)
                    .AddJsonFile($"connectionStrings.{env.EnvironmentName}.json", true, true)
                    .AddJsonFile("mailSettings.json", true, true)
                    .AddJsonFile($"mailSettings.{env.EnvironmentName}.json", true, true);

                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseStartup<Startup>();
        #endregion
    }
}
