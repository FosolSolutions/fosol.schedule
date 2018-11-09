using FluentValidation.AspNetCore;
using Fosol.Core.Extensions.ApplicationBuilders;
using Fosol.Core.Extensions.ServiceCollections;
using Fosol.Core.Mvc;
using Fosol.Schedule.API.Helpers;
using Fosol.Schedule.API.Helpers.Mail;
using Fosol.Schedule.DAL;
using Fosol.Schedule.Models.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Data.SqlClient;

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
        public IHostingEnvironment HostingEnvironment { get; }
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
            this.HostingEnvironment = env;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Startup>();

            _logger.LogInformation("Application starting");

            if (configuration["KeyVault:IsLoaded"] == "false")
            {
                _logger.LogWarning("KeyVault was not configured.");
            }
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
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = "CoEvent";
                    options.LoginPath = "/auth/signin";
                    options.LogoutPath = "/auth/signoff";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.AccessDeniedPath = "/auth/access/denied";
                    options.ClaimsIssuer = "CoEvent";
                    options.Events.OnRedirectToLogin = AuthenticationHelper.HandleOnRedirectToLogin;
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = this.Configuration["Authentication:Microsoft:ClientId"];
                    options.ClientSecret = this.Configuration["Authentication:Microsoft:ClientSecret"];
                    options.AuthorizationEndpoint = this.Configuration["Authentication:Microsoft:AuthorizationEndpoint"];
                    options.TokenEndpoint = this.Configuration["Authentication:Microsoft:TokenEndpoint"];
                    options.SaveTokens = true;
                    options.Scope.Add("offline_access");
                    options.Events = new OAuthEvents()
                    {
                        OnRemoteFailure = AuthenticationHelper.HandleOnRemoteFailure,
                        OnTicketReceived = AuthenticationHelper.HandleOnTicketReceived
                    };
                })// You must first create an app with Google and add its ID and Secret to your user-secrets.
                  // https://console.developers.google.com/project
                  // https://developers.google.com/identity/protocols/OAuth2WebServer
                  // https://developers.google.com/+/web/people/
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    options.AuthorizationEndpoint += "?prompt=consent"; // Hack so we always get a refresh token, it only comes on the first authorization response
                    options.AccessType = "offline";
                    options.SaveTokens = true;
                    options.Events = new OAuthEvents()
                    {
                        OnRemoteFailure = AuthenticationHelper.HandleOnRemoteFailure,
                        OnTicketReceived = AuthenticationHelper.HandleOnTicketReceived
                    };
                    options.ClaimActions.MapJsonSubKey("urn:google:image", "image", "url");
                    //options.ClaimActions.Remove(ClaimTypes.GivenName);
                });

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.Name = "CoEvent";
            //    options.LoginPath = "/auth/signin";
            //    options.LogoutPath = "/auth/signoff";
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //    options.Cookie.SameSite = SameSiteMode.None;
            //    options.AccessDeniedPath = "/auth/access/denied";
            //    options.ClaimsIssuer = "CoEvent";
            //});

            services.AddMvc(options =>
            {
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AccountValidator>());

            services.AddResponseHeaders(options =>
            {
                options.AddDefaultSecurePolicy();
                options.AddCustomHeader("Environment", this.HostingEnvironment.EnvironmentName);
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
                    var origins = Environment.GetEnvironmentVariable("CORS_ORIGINS")?.Split(" ");
                    if (origins != null) builder.WithOrigins(origins);
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDataSource(optionsBuilder =>
            {
                optionsBuilder.AddPrincipalAccessor<PrincipalAccessor>(services);
                if (this.HostingEnvironment.IsDevelopment())
                {
                    optionsBuilder.EnableSensitiveDataLogging();
                }
                var connectionString = this.Configuration.GetConnectionString("Schedule") ?? @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0";
                var builder = new SqlConnectionStringBuilder(connectionString);
                var password = this.Configuration["Database:Schedule:Password"];
                if (!String.IsNullOrWhiteSpace(password))
                {
                    builder.Password = password;
                }
                optionsBuilder.UseApplicationServiceProvider(services.BuildServiceProvider());
                optionsBuilder.UseLoggerFactory(_loggerFactory);
                optionsBuilder.UseSqlServer(builder.ConnectionString);
                //services.AddDataSourcePool(options =>
                //{
                //    options.UseSqlServer(builder.ConnectionString);
                //});
                //optionsBuilder.UseInMemoryDatabase("Schedule", options => { });
            });

            var mailOptions = new MailOptions();
            var mailConfig = this.Configuration.GetSection("Mail");
            mailConfig.Bind(mailOptions);
            services.Configure<MailOptions>(mailConfig);
            services.Configure<MailOptions>(o =>
            {
                o.AccountPassword = this.Configuration["Mail:AccountPassword"];
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                //options.HttpsPort = 443;
            });

            services.AddMailClient();
            services.AddSingleton<JsonErrorHandler>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() && false)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            app.UseDataSource();
            /***
            * Forwarded Headers were required for nginx at some point.
            * https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-2.1#nginx-configuration
            ***/
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                RequireHeaderSymmetry = false,
                ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
            });
            app.UseResponseHeaders();
            app.UseCors(env.EnvironmentName);
            app.UseHttpsRedirection();
            app.UseJsonExceptionMiddleware();
            //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseAuthentication();
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
