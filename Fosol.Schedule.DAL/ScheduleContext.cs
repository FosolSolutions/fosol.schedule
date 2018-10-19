﻿using Fosol.Schedule.Entities;
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
        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountRole> AccountRoles { get; set; }

        public DbSet<Calendar> Calendars { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<CriteriaObject> Criteria { get; set; }

        public DbSet<Entities.Attribute> Attributes { get; set; }

        public DbSet<Opening> Openings { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<ContactInfo> ContactInfos { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }
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
            #region Subscription
            modelBuilder.Entity<Subscription>()
                .ToTable("Subscriptions");

            modelBuilder.Entity<Subscription>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Subscription>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Subscription>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion

            #region Account
            modelBuilder.Entity<Account>()
                .ToTable("Accounts");

            modelBuilder.Entity<Account>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Account>()
                .HasOne(m => m.Owner)
                .WithMany(m => m.OwnedAccounts)
                .HasForeignKey(m => m.OwnerId)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Account>()
                .HasOne(m => m.Subscription)
                .WithMany(m => m.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Account>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Account>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Account>()
                .HasMany(m => m.Users)
                .WithOne(m => m.Account)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<AccountUser>()
                .ToTable("AccountUsers");

            modelBuilder.Entity<AccountUser>()
                .HasKey(m => new { m.AccountId, m.UserId });
            #endregion

            #region User
            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<User>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>()
                .HasOne(m => m.Info)
                .WithOne(m => m.User)
                .HasForeignKey<UserInfo>(m => m.UserId)
                .HasPrincipalKey<User>(m => m.Id);

            modelBuilder.Entity<User>()
                .HasMany(m => m.Roles)
                .WithOne(m => m.User)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>()
                .HasMany(m => m.Addresses)
                .WithOne(m => m.User)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>()
                .HasMany(m => m.ContactInformation)
                .WithOne(m => m.User)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>()
                .HasMany(m => m.Participants)
                .WithOne(m => m.User)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<UserAccountRole>()
                .ToTable("UserAccountRoles");

            modelBuilder.Entity<UserAccountRole>()
                .HasKey(m => new { m.UserId, m.AccountRoleId });

            modelBuilder.Entity<UserAddress>()
                .ToTable("UserAddresses");

            modelBuilder.Entity<UserAddress>()
                .HasKey(m => new { m.UserId, m.AddressId });

            modelBuilder.Entity<UserContactInfo>()
                .ToTable("UserContactInfos");

            modelBuilder.Entity<UserContactInfo>()
                .HasKey(m => new { m.UserId, m.ContactInfoId });
            #endregion

            #region Participant
            modelBuilder.Entity<Participant>()
                .ToTable("Participants");

            modelBuilder.Entity<Participant>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Participant>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Participant>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Participant>()
                .HasOne(m => m.User)
                .WithMany(m => m.Participants)
                .HasForeignKey(m => m.UserId)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Participant>()
                .HasMany(m => m.Addresses)
                .WithOne(m => m.Participant)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Participant>()
                .HasMany(m => m.Attributes)
                .WithOne(m => m.Participant)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Participant>()
                .HasMany(m => m.ContactInfos)
                .WithOne(m => m.Participant)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ParticipantAddress>()
                .ToTable("ParticipantAddresses");

            modelBuilder.Entity<ParticipantAddress>()
                .HasKey(m => new { m.ParticipantId, m.AddressId });

            modelBuilder.Entity<ParticipantAttribute>()
                .ToTable("ParticipantAttributes");

            modelBuilder.Entity<ParticipantAttribute>()
                .HasKey(m => new { m.ParticipantId, m.AttributeId });

            modelBuilder.Entity<ParticipantContactInfo>()
                .ToTable("ParticipantContactInfos");

            modelBuilder.Entity<ParticipantContactInfo>()
                .HasKey(m => new { m.ParticipantId, m.ContactInfoId });
            #endregion

            #region Calendar
            modelBuilder.Entity<Calendar>()
                .ToTable("Calendars");

            modelBuilder.Entity<Calendar>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Calendar>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Calendar>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Calendar>()
                .HasMany(m => m.Events)
                .WithOne(m => m.Calendar)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Calendar>()
                .HasMany(m => m.Participants)
                .WithOne(m => m.Calendar)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Calendar>()
                .HasMany(m => m.Criteria)
                .WithOne(m => m.Calendar)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Calendar>()
                .HasMany(m => m.Attributes)
                .WithOne(m => m.Calendar)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<CalendarCriteria>()
                .ToTable("CalendarCriteria");

            modelBuilder.Entity<CalendarCriteria>()
                .HasKey(m => new { m.CalendarId, m.CriteriaId });

            modelBuilder.Entity<CalendarAttribute>()
                .ToTable("CalendarAttributes");

            modelBuilder.Entity<CalendarAttribute>()
                .HasKey(m => new { m.CalendarId, m.AttributeId });
            #endregion

            #region Event
            modelBuilder.Entity<Event>()
                .ToTable("Events");

            modelBuilder.Entity<Event>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Event>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Event>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Event>()
                .HasMany(m => m.Activities)
                .WithOne(m => m.Event)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Event>()
                .HasMany(m => m.Tags)
                .WithOne(m => m.Event)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Event>()
                .HasMany(m => m.Criteria)
                .WithOne(m => m.Event)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<EventTag>()
                .ToTable("EventTags");

            modelBuilder.Entity<EventTag>()
                .HasKey(m => new { m.EventId, m.TagKey, m.TagValue });

            modelBuilder.Entity<EventCriteria>()
                .ToTable("EventCriteria");

            modelBuilder.Entity<EventCriteria>()
                .HasKey(m => new { m.EventId, m.CriteriaId });
            #endregion

            #region Activity
            modelBuilder.Entity<Activity>()
                .ToTable("Activities");

            modelBuilder.Entity<Activity>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Activity>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Activity>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Activity>()
                .HasMany(m => m.Criteria)
                .WithOne(m => m.Activity)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ActivityCriteria>()
                .ToTable("ActivityCriteria");

            modelBuilder.Entity<ActivityCriteria>()
                .HasKey(m => new { m.ActivityId, m.CriteriaId });
            #endregion

            #region Opening
            modelBuilder.Entity<Opening>()
                .ToTable("Openings");

            modelBuilder.Entity<Opening>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Opening>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Opening>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Opening>()
                .HasMany(m => m.Participants)
                .WithOne(m => m.Opening)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Opening>()
                .HasMany(m => m.Applications)
                .WithOne(m => m.Opening)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Opening>()
                .HasMany(m => m.Criteria)
                .WithOne(m => m.Opening)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<OpeningParticipant>()
                .ToTable("OpeningParticipants");

            modelBuilder.Entity<OpeningParticipant>()
                .HasKey(m => new { m.OpeningId, m.ParticipantId });

            modelBuilder.Entity<OpeningParticipantApplication>()
                .ToTable("OpeningParticipantApplications");

            modelBuilder.Entity<OpeningParticipantApplication>()
                .HasKey(m => new { m.OpeningId, m.ParticipantId });

            modelBuilder.Entity<OpeningCriteria>()
                .ToTable("OpeningCriteria");

            modelBuilder.Entity<OpeningCriteria>()
                .HasKey(m => new { m.OpeningId, m.CriteriaId });
            #endregion

            #region Address
            modelBuilder.Entity<Address>()
                .ToTable("Addresses");

            modelBuilder.Entity<Address>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Address>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Address>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion

            #region Attribute
            modelBuilder.Entity<Entities.Attribute>()
                .ToTable("Attributes");

            modelBuilder.Entity<Entities.Attribute>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Entities.Attribute>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Entities.Attribute>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion

            #region ContactInfo
            modelBuilder.Entity<ContactInfo>()
                .ToTable("ContactInfos");

            modelBuilder.Entity<ContactInfo>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ContactInfo>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ContactInfo>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion

            #region Tag
            modelBuilder.Entity<Tag>()
                .ToTable("Tags");

            modelBuilder.Entity<Tag>()
                .HasKey(m => new { m.Key, m.Value });

            modelBuilder.Entity<Tag>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Tag>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion

            #region Criteria
            modelBuilder.Entity<CriteriaObject>()
                .ToTable("Criteria");

            modelBuilder.Entity<CriteriaObject>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CriteriaObject>()
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<CriteriaObject>()
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            #endregion

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
