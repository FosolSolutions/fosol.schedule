using AutoMapper;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.DAL.Maps;
using Fosol.Schedule.DAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;

namespace Fosol.Schedule.DAL
{
    // TODO: Controll Tracking
    // TODO: Controll size of models (read and write models)
    /// <summary>
    /// DataSource sealed class, provides a way to interact with the datasource.
    /// </summary>
    public sealed class DataSource : IDataSource
    {
        #region Variables
        private readonly Lazy<IHelperService> _helperService;
        private readonly Lazy<ISubscriptionService> _subscriptionService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<ICalendarService> _calendarService;
        private readonly Lazy<IParticipantService> _participantService;
        private readonly Lazy<IEventService> _eventService;
        private readonly Lazy<IActivityService> _activityService;
        private readonly Lazy<IOpeningService> _openingService;
        private readonly Lazy<IScheduleService> _scheduleService;
        #endregion

        #region Properties
        /// <summary>
        /// get - The DbContext used to communicate with the datasource.
        /// </summary>
        internal ScheduleContext Context { get; }

        /// <summary>
        /// get - The AutoMapper used to cast objects that will be added to the datasource.
        /// </summary>
        public IMapper AddMapper { get; }

        /// <summary>
        /// get - The AutoMapper used to cast objects that will be updated in the datasource.
        /// </summary>
        public IMapper UpdateMapper { get; }

        /// <summary>
        /// get - The current principal using the datasource.
        /// </summary>
        public IPrincipal Principal { get; }

        /// <summary>
        /// get - A helper service with various functions.
        /// </summary>
        public IHelperService Helper { get { return _helperService.Value; } }

        /// <summary>
        /// get - The service to manage subscriptions.
        /// </summary>
        public ISubscriptionService Subscriptions { get { return _subscriptionService.Value; } }

        /// <summary>
        /// get - The service to manage users.
        /// </summary>
        public IUserService Users { get { return _userService.Value; } }

        /// <summary>
        /// get - The service to manage accounts.
        /// </summary>
        public IAccountService Accounts { get { return _accountService.Value; } }

        /// <summary>
        /// get - The service to manage calendars.
        /// </summary>
        public ICalendarService Calendars { get { return _calendarService.Value; } }

        /// <summary>
        /// get - The service to manage participants.
        /// </summary>
        public IParticipantService Participants { get { return _participantService.Value; } }

        /// <summary>
        /// get - The service to manage events.
        /// </summary>
        public IEventService Events { get { return _eventService.Value; } }

        /// <summary>
        /// get - The service to manage activities.
        /// </summary>
        public IActivityService Activities { get { return _activityService.Value; } }

        /// <summary>
        /// get - The service to manage openings.
        /// </summary>
        public IOpeningService Openings { get { return _openingService.Value; } }

        /// <summary>
        /// get - The service to manage schedules.
        /// </summary>
        public IScheduleService Schedules { get { return _scheduleService.Value; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DataSource object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="addProfile"></param>
        /// <param name="updateProfile"></param>
        DataSource(IHttpContextAccessor httpContext, IProfileAddMap addProfile, IProfileUpdateMap updateProfile)
        {
            this.Principal = httpContext.HttpContext?.User;
            this.AddMapper = new MapperConfiguration(config =>
            {
                addProfile.BindDataSource(this);
                config.AddProfile((Profile)addProfile);
            }).CreateMapper();
            this.UpdateMapper = new MapperConfiguration(config =>
            {
                updateProfile.BindDataSource(this);
                config.AddProfile((Profile)updateProfile);
            }).CreateMapper();

            // TODO: reflection to auto initialize the services.
            _helperService = new Lazy<IHelperService>(() => new HelperService(this));
            _subscriptionService = new Lazy<ISubscriptionService>(() => new SubscriptionService(this));
            _userService = new Lazy<IUserService>(() => new UserService(this));
            _accountService = new Lazy<IAccountService>(() => new AccountService(this));
            _calendarService = new Lazy<ICalendarService>(() => new CalendarService(this));
            _participantService = new Lazy<IParticipantService>(() => new ParticipantService(this));
            _eventService = new Lazy<IEventService>(() => new EventService(this));
            _activityService = new Lazy<IActivityService>(() => new ActivityService(this));
            _openingService = new Lazy<IOpeningService>(() => new OpeningService(this));
            _scheduleService = new Lazy<IScheduleService>(() => new ScheduleService(this));
        }

        /// <summary>
        /// Creates a new instance of a DataSource object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContext"></param>
        /// <param name="addProfile"></param>
        /// <param name="updateProfile"></param>
        internal DataSource(DbContextOptions<ScheduleContext> options, IHttpContextAccessor httpContext, IProfileAddMap addProfile, IProfileUpdateMap updateProfile) : this(httpContext, addProfile, updateProfile)
        {
            this.Context = new ScheduleContext(options);
        }

        /// <summary>
        /// Creates a new instance of a DataSource object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContext"></param>
        /// <param name="addProfile"></param>
        /// <param name="updateProfile"></param>
        public DataSource(DbContextOptions options, IHttpContextAccessor httpContext, IProfileAddMap addProfile, IProfileUpdateMap updateProfile) : this(httpContext, addProfile, updateProfile)
        {
            this.Context = new ScheduleContext(options);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Applies any pending migrations for the context to the database.  Will create the database if it does not already exist.
        /// </summary>
        public void Migrate()
        {
            this.Context.Database.Migrate();
        }

        /// <summary>
        /// Ensures that the database for the context exists.  If it exists, no action is taken.  IF it does not exist then the database adn all its schema are created.  If the database exists, then no effort is made to ensure it is compatible with the model for this context.
        /// </summary>
        /// <returns></returns>
        public bool EnsureCreated()
        {
            return this.Context.Database.EnsureCreated();
        }

        /// <summary>
        /// Ensures that the database for the context does not exist.  If it does not exist, no action is taken.  If it does exist then the database is deleted.
        /// </summary>
        /// <returns></returns>
        public bool EnsureDeleted()
        {
            return this.Context.Database.EnsureDeleted();
        }

        /// <summary>
        /// Commit the in-memory changes to the datasource.
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            var result = this.Context.SaveChanges();
            Sync();
            return result;
        }

        /// <summary>
        /// Commit the in-memory changes to the datasource within an single transaction.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int CommitTransaction()
        {
            return this.CommitTransaction(this.Context.SaveChanges);
        }

        /// <summary>
        /// Commit the in-memory changes to the datasource within an single transaction.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int CommitTransaction(Func<int> action)
        {
            int result;
            using (var transaction = this.Context.Database.BeginTransaction())
            {
                try
                {
                    result = action?.Invoke() ?? this.Context.SaveChanges();

                    transaction.Commit();

                    Sync();
                }
                catch (DbUpdateException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Commit the in-memory changes to the datasource within an single transaction.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public void CommitTransaction(Action action)
        {
            this.CommitTransaction(() => { action(); return 0; });
        }

        /// <summary>
        /// Sync every active service that contains tracked entities and models.
        /// This will update each model with their referenced entity after an update.
        /// </summary>
        private void Sync()
        {
            // TODO: Rewrite to be better performance.
            var type = this.GetType();
            var gtype = typeof(Lazy<>);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Where(f => f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == gtype).ToArray();

            foreach (var field in fields)
            {
                var stype = field.FieldType.GetGenericArguments()[0];
                var gstype = gtype.MakeGenericType(stype);
                var lazyService = field.GetValue(this);
                var isValueCreatedProp = gstype.GetProperty(nameof(Lazy<object>.IsValueCreated));
                var valueProp = gstype.GetProperty(nameof(Lazy<object>.Value));

                if ((bool)isValueCreatedProp.GetValue(lazyService))
                {
                    var service = valueProp.GetValue(lazyService);
                    var sync = service.GetType().GetMethod("Sync", BindingFlags.Instance | BindingFlags.NonPublic);

                    if (sync != null)
                    {
                        sync.Invoke(service, null);
                    }
                }
            }
        }
        #endregion
    }
}
