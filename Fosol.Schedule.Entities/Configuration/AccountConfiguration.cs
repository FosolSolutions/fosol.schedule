﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts"); // TODO: Add schema and make it DI configurable.
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Key).IsRequired();
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.Subscription).WithMany(m => m.Accounts).HasForeignKey(m => m.SubscriptionId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.Owner).WithMany(m => m.OwnedAccounts).HasForeignKey(m => m.OwnerId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Key }).IsUnique();
            builder.HasIndex(m => new { m.OwnerId, m.State });
        }
        #endregion
    }
}
