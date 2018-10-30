using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder
                .ToTable("Activities");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Key })
                .IsUnique();

            builder
                .HasIndex(m => new { m.EventId, m.State, m.StartOn, m.EndOn, m.Name });

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
                .HasMany(m => m.Criteria)
                .WithOne(m => m.Activity)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
