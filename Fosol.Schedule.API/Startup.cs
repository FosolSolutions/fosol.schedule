﻿using Fosol.Core.Extensions.ApplicationBuilders;
using Fosol.Core.Extensions.ServiceCollections;
using Fosol.Schedule.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fosol.Schedule.API
{
    /// <summary>
    /// Startup class, provides a way to start the web application.
    /// </summary>
    public class Startup
    {
        #region Variables
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        #endregion

        #region Properties
        /// <summary>
        /// get - The program configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// get - The hosting environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Startup object, and intializes it with the specified arguments.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        /// <param name="loggerFactory"></param>
        public Startup(IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.Configuration = configuration;
            this.Environment = env;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Startup>();

            _logger.LogInformation("Application starting");
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = "fosol.schedule";
                    options.LoginPath = "/auth/signin";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.None;
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/auth/signin";
            });

            services.AddMvc(options =>
            {

            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddResponseHeaders(options =>
            {
                options.AddDefaultSecurePolicy();
                options.AddCustomHeader("Environment", this.Environment.EnvironmentName);
                options.AddCustomHeader("Content-Language", "en-US");
            });

            services.AddCors(options =>
            {
                options.AddPolicy(EnvironmentName.Development, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
                options.AddPolicy(EnvironmentName.Staging, builder =>
                {
                    builder.WithOrigins("https://localhost:44374", "https://localhost:5001")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDataSource(optionsBuilder =>
            {
                var connectionString = this.Configuration.GetConnectionString("Schedule") ?? @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0";
                optionsBuilder.UseApplicationServiceProvider(services.BuildServiceProvider());
                optionsBuilder.UseLoggerFactory(_loggerFactory);
                optionsBuilder.UseSqlServer(connectionString);
                if (this.Environment.IsDevelopment())
                {
                    optionsBuilder.EnableSensitiveDataLogging();
                }
                //optionsBuilder.UseInMemoryDatabase("Schedule", options => { });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            app.UseDataSource();
            app.UseResponseHeaders();
            app.UseCors(env.EnvironmentName);
            app.UseHttpsRedirection();
            //app.UseJsonExceptionMiddleware();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc(options =>
            {
                options.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Calendar", action = "Index" }
                );
                options.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
        #endregion
    }
}
