using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class OpeningConfiguration : IEntityTypeConfiguration<Opening>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Opening> builder)
        {
            builder
                .ToTable("Openings");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Key })
                .IsUnique();

            builder
                .HasIndex(m => new { m.ActivityId, m.State, m.OpeningType, m.ApplicationProcess, m.Name });

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
                .HasMany(m => m.Participants)
                .WithOne(m => m.Opening)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Applications)
                .WithOne(m => m.Opening)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Criteria)
                .WithOne(m => m.Opening)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
