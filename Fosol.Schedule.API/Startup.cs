using FluentValidation.AspNetCore;
using Fosol.Core.Extensions.ApplicationBuilders;
using Fosol.Core.Extensions.ServiceCollections;
using Fosol.Schedule.API.Helpers;
using Fosol.Schedule.API.Helpers.Mail;
using Fosol.Schedule.DAL;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Models.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.Text.Encodings.Web;
using System.Threading.Tasks;

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
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = this.Configuration["Authentication:Microsoft:ClientId"];
                    options.ClientSecret = this.Configuration["Authentication:Microsoft:ClientSecret"];
                    options.AuthorizationEndpoint = "https://login.microsoftonline.com/270632f2-6258-4bc4-80b3-435c63f379cf/oauth2/v2.0/authorize";
                    options.TokenEndpoint = "https://login.microsoftonline.com/270632f2-6258-4bc4-80b3-435c63f379cf/oauth2/v2.0/token";
                    options.SaveTokens = true;
                    options.Scope.Add("offline_access");
                    options.Events = new OAuthEvents()
                    {
                        OnRemoteFailure = HandleOnRemoteFailure,
                        OnTicketReceived = HandleOnTicketReceived
                    };
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "CoEvent";
                options.LoginPath = "/auth/signin";
                options.LogoutPath = "/auth/signoff";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.None;
                options.AccessDeniedPath = "/auth/access/denied";
                options.ClaimsIssuer = "CoEvent";
            });

            services.AddMvc(options =>
            {
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
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


        // TODO: Replace with better implementation, use built-in error handling.
        private async Task HandleOnRemoteFailure(RemoteFailureContext context)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<html><body>");
            await context.Response.WriteAsync("A remote failure has occurred: " + UrlEncoder.Default.Encode(context.Failure.Message) + "<br>");

            if (context.Properties != null)
            {
                await context.Response.WriteAsync("Properties:<br>");
                foreach (var pair in context.Properties.Items)
                {
                    await context.Response.WriteAsync($"-{ UrlEncoder.Default.Encode(pair.Key)}={ UrlEncoder.Default.Encode(pair.Value)}<br>");
                }
            }

            await context.Response.WriteAsync("<a href=\"/\">Home</a>");
            await context.Response.WriteAsync("</body></html>");

            // context.Response.Redirect("/error?FailureMessage=" + UrlEncoder.Default.Encode(context.Failure.Message));

            context.HandleResponse();
        }

        private async Task HandleOnTicketReceived(TicketReceivedContext context)
        {
            await Task.Run(() =>
            {
                var datasource = context.HttpContext.RequestServices.GetRequiredService<IDataSource>();
                AuthenticationHelper.AuthorizeOauthUser(datasource, context.Principal);
            });
            context.Success();
        }
        #endregion
    }
}
