using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserSetting> builder)
        {
            builder
                .ToTable("UserSettings");

            builder
                .Property(m => m.UserId)
                .ValueGeneratedNever();

            builder
                .HasIndex(m => new { m.UserId, m.Key })
                .IsUnique(true);

            builder
                .HasOne(m => m.User)
                .WithMany(m => m.Settings);

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
        }
        #endregion
    }
}
