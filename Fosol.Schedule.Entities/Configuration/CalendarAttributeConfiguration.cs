using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class CalendarAttributeConfiguration : IEntityTypeConfiguration<CalendarAttribute>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<CalendarAttribute> builder)
        {

            builder
                .ToTable("CalendarAttributes");

            builder
                .HasKey(m => new { m.CalendarId, m.AttributeId });
        }
        #endregion
    }
}
