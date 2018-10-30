using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class EventCriteriaConfiguration : IEntityTypeConfiguration<EventCriteria>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<EventCriteria> builder)
        {

            builder
                .ToTable("EventCriteria");

            builder
                .HasKey(m => new { m.EventId, m.CriteriaId });
        }
        #endregion
    }
}
