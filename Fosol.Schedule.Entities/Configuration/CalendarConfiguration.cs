using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Calendar> builder)
        {
            builder
                .ToTable("Calendars");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Key })
                .IsUnique();

            builder
                .HasIndex(m => new { m.Name, m.State });

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
                .HasMany(m => m.Events)
                .WithOne(m => m.Calendar)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Participants)
                .WithOne(m => m.Calendar)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Criteria)
                .WithOne(m => m.Calendar)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Attributes)
                .WithOne(m => m.Calendar)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
