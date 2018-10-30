using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class EventTagConfiguration : IEntityTypeConfiguration<EventTag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<EventTag> builder)
        {

            builder
                .ToTable("EventTags");

            builder
                .HasKey(m => new { m.EventId, m.TagKey, m.TagValue });
        }
        #endregion
    }
}
