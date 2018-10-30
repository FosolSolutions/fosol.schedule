using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class CalendarTagConfiguration : IEntityTypeConfiguration<CalendarTag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<CalendarTag> builder)
        {
            builder.ToTable("CalendarTags");

            builder.HasKey(m => new { m.CalendarId, m.TagKey, m.TagValue });

            builder.HasOne(m => m.Calendar).WithMany(m => m.Tags).HasForeignKey(m => m.CalendarId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Tag).WithMany(m => m.Calendars).HasForeignKey(m => new { m.TagKey, m.TagValue }).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
