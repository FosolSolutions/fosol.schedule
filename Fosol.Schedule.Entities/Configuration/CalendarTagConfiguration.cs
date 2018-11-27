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

            builder.HasKey(m => new { m.CalendarId, m.Key, m.Value });

            builder.HasOne(m => m.Calendar).WithMany(m => m.Tags).HasForeignKey(m => m.CalendarId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
