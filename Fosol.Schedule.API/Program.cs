using System;
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
        #region Variables
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var build = CreateWebHostBuilder(args).Build();
            build.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, builder) =>
                {
                    var env = builderContext.HostingEnvironment;
                    builder
                        .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("connectionStrings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"connectionStrings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("mailSettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"mailSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();

                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            builder.AddUserSecrets(appAssembly, optional: true);
                        }
                    }
                    else
                    {
                        var config = builder.Build();
                        builder.AddAzureKeyVault(config["KeyVault:Endpoint"], config["KeyVault:ClientId"], config["KeyVault:ClientSecret"]);
                    }

                    if (args != null)
                    {
                        builder.AddCommandLine(args);
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
