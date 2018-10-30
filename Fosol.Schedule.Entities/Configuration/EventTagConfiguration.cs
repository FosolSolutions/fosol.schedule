using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class EventTagConfiguration : IEntityTypeConfiguration<EventTag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<EventTag> builder)
        {
            builder.ToTable("EventTags");

            builder.HasKey(m => new { m.EventId, m.TagKey, m.TagValue });

            builder.HasOne(m => m.Event).WithMany(m => m.Tags).HasForeignKey(m => m.EventId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Tag).WithMany(m => m.Events).HasForeignKey(m => new { m.TagKey, m.TagValue }).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
