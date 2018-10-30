using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(m => m.Id);
            builder.ToTable("Schedules");
        }
        #endregion
    }
}
