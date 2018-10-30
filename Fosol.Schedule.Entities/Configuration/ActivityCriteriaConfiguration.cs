using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ActivityCriteriaConfiguration : IEntityTypeConfiguration<ActivityCriteria>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ActivityCriteria> builder)
        {

            builder
                .ToTable("ActivityCriteria");

            builder
                .HasKey(m => new { m.ActivityId, m.CriteriaId });
        }
        #endregion
    }
}
