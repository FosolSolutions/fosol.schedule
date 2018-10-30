using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class CalendarCriteriaConfiguration : IEntityTypeConfiguration<CalendarCriteria>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<CalendarCriteria> builder)
        {

            builder
                .ToTable("CalendarCriteria");

            builder
                .HasKey(m => new { m.CalendarId, m.CriteriaId });
        }
        #endregion
    }
}
