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
  /// Program class, the CoEvent web API application.
  /// </summary>
  public class Program
  {
    #region Methods
    /// <summary>
    /// The default method that is called to start the application.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
      var build = CreateWebHostBuilder(args).Build();
      build.Run();
    }

    /// <summary>
    /// Configure the web application.
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
                      .AddJsonFile("connectionStrings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"connectionStrings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                      .AddJsonFile("mailSettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"mailSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();

              if (env.IsDevelopment() || env.IsLocal())
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
                if (!String.IsNullOrWhiteSpace(config["KeyVault:Endpoint"]) && !String.IsNullOrWhiteSpace(config["KeyVault:ClientId"]) && !String.IsNullOrWhiteSpace(config["KeyVault:ClientSecret"]))
                {
                  builder.AddAzureKeyVault(config["KeyVault:Endpoint"], config["KeyVault:ClientId"], config["KeyVault:ClientSecret"]);
                  config["KeyVault:IsLoaded"] = "true";
                }
                else
                {
                  config["KeyVault:IsLoaded"] = "false";
                }
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
