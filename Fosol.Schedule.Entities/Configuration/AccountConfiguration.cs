using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(m => m.Id);
            builder.ToTable("Accounts"); // TODO: Add schema and make it DI configurable.

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Key })
                .IsUnique();

            builder
                .HasIndex(m => new { m.OwnerId, m.State });

            builder
                .HasOne(m => m.Owner)
                .WithMany(m => m.OwnedAccounts)
                .HasForeignKey(m => m.OwnerId)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(m => m.Subscription)
                .WithMany(m => m.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Users)
                .WithOne(m => m.Account)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
