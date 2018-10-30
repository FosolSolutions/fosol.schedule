using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .ToTable("Events");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Key })
                .IsUnique();

            builder
                .HasIndex(m => new { m.CalendarId, m.State, m.StartOn, m.EndOn, m.Name });

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
                .HasMany(m => m.Activities)
                .WithOne(m => m.Event)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Tags)
                .WithOne(m => m.Event)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Criteria)
                .WithOne(m => m.Event)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
