using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class CalendarAttributeConfiguration : IEntityTypeConfiguration<CalendarAttribute>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<CalendarAttribute> builder)
        {
            builder.ToTable("CalendarAttributes");

            builder.HasKey(m => new { m.CalendarId, m.AttributeId });

            builder.HasOne(m => m.Calendar).WithMany(m => m.Attributes).HasForeignKey(m => m.CalendarId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Attribute).WithMany().HasForeignKey(m => m.AttributeId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
