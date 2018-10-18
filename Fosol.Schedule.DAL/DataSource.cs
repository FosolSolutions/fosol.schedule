using AutoMapper;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.DAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Transactions;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// DataSource sealed class, provides a way to interact with the datasource.
    /// </summary>
    public sealed class DataSource : IDataSource
    {
        #region Variables
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<ICalendarService> _calendarService;
        private readonly Lazy<IParticipantService> _participantService;
        private readonly Lazy<IEventService> _eventService;
        private readonly Lazy<IActivityService> _activityService;
        #endregion

        #region Properties
        /// <summary>
        /// get - The DbContext used to communicate with the datasource.
        /// </summary>
        internal ScheduleContext Context { get; }

        /// <summary>
        /// get - The AutoMapper used to cast objects.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// get - The current principal using the datasource.
        /// </summary>
        public IPrincipal Principal { get; }

        /// <summary>
        /// get - The service to manage users.
        /// </summary>
        public IUserService Users { get { return _userService.Value; } }

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
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DataSource object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="httpContext"></param>
        DataSource(IHttpContextAccessor httpContext)
        {
            this.Principal = httpContext.HttpContext?.User;
            this.Mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<Entities.User, Models.User>();
                config.CreateMap<Entities.Calendar, Models.Calendar>();
                config.CreateMap<Models.Calendar, Entities.Calendar>();
                config.CreateMap<Entities.Participant, Models.Participant>();
                config.CreateMap<Entities.Event, Models.Event>();
                config.CreateMap<Entities.Activity, Models.Activity>();
                config.CreateMap<Entities.Attribute, Models.Attribute>();
            }).CreateMapper();

            _userService = new Lazy<IUserService>(() => new UserService(this));
            _calendarService = new Lazy<ICalendarService>(() => new CalendarService(this));
            _participantService = new Lazy<IParticipantService>(() => new ParticipantService(this));
            _eventService = new Lazy<IEventService>(() => new EventService(this));
            _activityService = new Lazy<IActivityService>(() => new ActivityService(this));
        }

        /// <summary>
        /// Creates a new instance of a DataSource object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContext"></param>
        internal DataSource(DbContextOptions<ScheduleContext> options, IHttpContextAccessor httpContext) : this(httpContext)
        {
            this.Context = new ScheduleContext(options);
        }

        /// <summary>
        /// Creates a new instance of a DataSource object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContext"></param>
        public DataSource(DbContextOptions options, IHttpContextAccessor httpContext) : this(httpContext)
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

        private void Sync()
        {
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
