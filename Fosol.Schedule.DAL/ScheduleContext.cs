using Fosol.Schedule.DAL.Helpers;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// ScheduleContext private sealed class, provides a way connect to the datasource.
    /// </summary>
    sealed class ScheduleContext : DbContext
    {
        #region Variables
        private readonly string _connectionString;
        #endregion

        #region Properties
        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Calendar> Calendars { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Criteria> Criterias { get; set; }

        public DbSet<Attribute> Attributes { get; set; }

        public DbSet<Opening> Openings { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<ContactInfo> ContactInfos { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ScheduleContext object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="connectionString"></param>
        public ScheduleContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new instance of a ScheduleContext object, and initializes it with the specified configuration options.
        /// </summary>
        /// <param name="options"></param>
        public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options)
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
                optionsBuilder.UseSqlServer(_connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Creates the datasource.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Account
            modelBuilder.Entity<Account>()
                .HasOne(m => m.Owner)
                .WithMany(m => m.OwnedAccounts)
                .HasForeignKey(m => m.OwnerId)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Account>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Account>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<AccountUser>()
                .HasKey(m => new { m.AccountId, m.UserId });
            #endregion

            #region User
            modelBuilder.Entity<User>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<User>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<User>()
                .HasOne(m => m.Info)
                .WithOne(m => m.User)
                .HasForeignKey<UserInfo>(m => m.UserId)
                .HasPrincipalKey<User>(m => m.Id);

            modelBuilder.Entity<UserAccountRole>()
                .HasKey(m => new { m.UserId, m.AccountRoleId });

            modelBuilder.Entity<UserAddress>()
                .HasKey(m => new { m.UserId, m.AddressId });

            modelBuilder.Entity<UserContactInfo>()
                .HasKey(m => new { m.UserId, m.ContactInfoId });

            modelBuilder.Entity<UserParticipant>()
                .HasKey(m => new { m.UserId, m.ParticipantId });
            #endregion

            #region Participant
            modelBuilder.Entity<Participant>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Participant>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Participant>()
                .HasOne(m => m.User)
                .WithMany(m => m.Participants)
                .HasForeignKey(m => m.UserId)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<ParticipantAddress>()
                .HasKey(m => new { m.ParticipantId, m.AddressId });

            modelBuilder.Entity<ParticipantAttribute>()
                .HasKey(m => new { m.ParticipantId, m.AttributeId });

            modelBuilder.Entity<ParticipantContactInfo>()
                .HasKey(m => new { m.ParticipantId, m.ContactInfoId });
            #endregion

            #region Calendar
            modelBuilder.Entity<Calendar>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Calendar>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<CalendarParticipant>()
                .HasKey(m => new { m.CalendarId, m.ParticipantId });

            modelBuilder.Entity<CalendarCriteria>()
                .HasKey(m => new { m.CalendarId, m.CriteriaId });

            modelBuilder.Entity<CalendarAttribute>()
                .HasKey(m => new { m.CalendarId, m.AttributeId });
            #endregion

            #region Event
            modelBuilder.Entity<Event>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Event>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<EventTag>()
                .HasKey(m => new { m.EventId, m.TagKey, m.TagValue });

            modelBuilder.Entity<EventCriteria>()
                .HasKey(m => new { m.EventId, m.CriteriaId });
            #endregion

            #region Activity
            modelBuilder.Entity<Activity>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Activity>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<ActivityCriteria>()
                .HasKey(m => new { m.ActivityId, m.CriteriaId });
            #endregion

            #region Opening
            modelBuilder.Entity<Opening>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Opening>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<OpeningParticipant>()
                .HasKey(m => new { m.OpeningId, m.ParticipantId });

            modelBuilder.Entity<OpeningParticipantApplication>()
                .HasKey(m => new { m.OpeningId, m.ParticipantId });

            modelBuilder.Entity<OpeningCriteria>()
                .HasKey(m => new { m.OpeningId, m.CriteriaId });
            #endregion

            #region Address
            modelBuilder.Entity<Address>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Address>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);
            #endregion

            #region Attribute
            modelBuilder.Entity<Attribute>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Attribute>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);
            #endregion

            #region ContactInfo
            modelBuilder.Entity<ContactInfo>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<ContactInfo>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);
            #endregion

            #region Tag
            modelBuilder.Entity<Tag>()
                .HasKey(m => new { m.Key, m.Value });

            modelBuilder.Entity<Tag>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Tag>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id);
            #endregion

            ScheduleContextData.Init(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
