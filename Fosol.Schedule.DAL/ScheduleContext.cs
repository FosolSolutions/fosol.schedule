using Fosol.Schedule.Entities;
using Fosol.Schedule.Entities.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// ScheduleContext private sealed class, provides a way connect to the datasource.
    /// </summary>
    class ScheduleContext : DbContext
    {
        #region Variables
        #endregion

        #region Properties

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }


        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityCriteria> ActivityCriteria { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Entities.Attribute> Attributes { get; set; }

        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CalendarCriteria> CalendarCriteria { get; set; }

        public DbSet<ContactInfo> ContactInfo { get; set; }

        public DbSet<CriteriaObject> Criteria { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventCriteria> EventCriteria { get; set; }

        public DbSet<Opening> Openings { get; set; }
        public DbSet<OpeningCriteria> OpeningCriteria { get; set; }
        public DbSet<OpeningParticipant> OpeningParticipants { get; set; }
        public DbSet<OpeningParticipantApplication> OpeningParticipantApplications { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ScheduleContext object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="options"></param>
        public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options)
        {
        }

        /// <summary>
        /// Creates a new instance of a ScheduleContext object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="options"></param>
        public ScheduleContext(DbContextOptions options) : base(options)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Configures the DbContext with the specified options.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
                //optionsBuilder.UseInMemoryDatabase("Schedule", options => { });
            }

            // var map = ScheduleMapper.Map; // Initialize mapping.

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Creates the datasource.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AccountUserConfiguration());

            modelBuilder.ApplyConfiguration(new ActivityConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityCriteriaConfiguration());

            modelBuilder.ApplyConfiguration(new AttributeConfiguration());

            modelBuilder.ApplyConfiguration(new CalendarAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new CalendarConfiguration());
            modelBuilder.ApplyConfiguration(new CalendarCriteriaConfiguration());

            modelBuilder.ApplyConfiguration(new ContactInfoConfiguration());

            modelBuilder.ApplyConfiguration(new CriteriaObjectConfiguration());

            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new EventCriteriaConfiguration());
            modelBuilder.ApplyConfiguration(new EventTagConfiguration());

            modelBuilder.ApplyConfiguration(new OpeningConfiguration());
            modelBuilder.ApplyConfiguration(new OpeningCriteriaConfiguration());
            modelBuilder.ApplyConfiguration(new OpeningParticipantApplicationConfiguration());
            modelBuilder.ApplyConfiguration(new OpeningParticipantConfiguration());

            modelBuilder.ApplyConfiguration(new ParticipantAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
            modelBuilder.ApplyConfiguration(new ParticipantContactInfoConfiguration());

            //modelBuilder.ApplyConfiguration(new ScheduleConfiguration());

            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());

            modelBuilder.ApplyConfiguration(new TagConfiguration());

            modelBuilder.ApplyConfiguration(new UserAccountRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserContactInfoConfiguration());
            modelBuilder.ApplyConfiguration(new UserInfoConfiguration());
            modelBuilder.ApplyConfiguration(new UserSettingConfiguration());

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(m => m.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime)))
            {
                property.Relational().ColumnType = "DATETIME2";

                if (property.Name == "AddedOn")
                {
                    property.Relational().DefaultValueSql = "GETUTCDATE()";
                }
            }

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
