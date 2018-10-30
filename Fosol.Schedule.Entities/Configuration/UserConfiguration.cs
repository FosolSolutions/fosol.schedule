using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable("Users");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Key })
                .IsUnique();

            builder
                .HasIndex(m => new { m.Email })
                .IsUnique();

            builder
                .HasIndex(m => new { m.State });

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
                .HasOne(m => m.Info)
                .WithOne(m => m.User)
                .HasForeignKey<UserInfo>(m => m.UserId)
                .HasPrincipalKey<User>(m => m.Id);

            builder
                .HasMany(m => m.Roles)
                .WithOne(m => m.User)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.ContactInformation)
                .WithOne(m => m.User)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Participants)
                .WithOne(m => m.User)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
