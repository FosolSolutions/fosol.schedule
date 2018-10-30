using Fosol.Schedule.DAL.Extensions;
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
        public DbSet<ActivityTag> ActivityTags { get; set; }

        public DbSet<Entities.Attribute> Attributes { get; set; }

        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CalendarCriteria> CalendarCriteria { get; set; }
        public DbSet<CalendarTag> CalendarTags { get; set; }

        public DbSet<ContactInfo> ContactInfo { get; set; }

        public DbSet<CriteriaObject> Criteria { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventCriteria> EventCriteria { get; set; }
        public DbSet<EventTag> EventTags { get; set; }

        public DbSet<Opening> Openings { get; set; }
        public DbSet<OpeningCriteria> OpeningCriteria { get; set; }
        public DbSet<OpeningParticipant> OpeningParticipants { get; set; }
        public DbSet<OpeningParticipantApplication> OpeningParticipantApplications { get; set; }
        public DbSet<OpeningTag> OpeningTags { get; set; }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantAttribute> ParticipantAttributes { get; set; }
        public DbSet<ParticipantContactInfo> ParticipantContactInfo { get; set; }

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

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Creates the datasource.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations(typeof(AccountConfiguration));

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(m => m.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime)))
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.Relational().ColumnType = "DATETIME2";

                    if (property.Name == nameof(BaseEntity.AddedOn))
                    {
                        property.Relational().DefaultValueSql = "GETUTCDATE()";
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
