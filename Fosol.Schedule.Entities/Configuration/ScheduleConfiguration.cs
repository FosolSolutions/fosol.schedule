﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Key).IsRequired();
            builder.Property(m => m.Name).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Description).HasMaxLength(2000);
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.Account).WithMany(m => m.Schedules).HasForeignKey(m => m.AccountId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Key }).IsUnique();
            builder.HasIndex(m => new { m.Name, m.State });
        }
        #endregion
    }
}
