using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ActivityTagConfiguration : IEntityTypeConfiguration<ActivityTag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ActivityTag> builder)
        {
            builder.ToTable("ActivityTags");

            builder.HasKey(m => new { m.ActivityId, m.TagKey, m.TagValue });

            builder.HasOne(m => m.Activity).WithMany(m => m.Tags).HasForeignKey(m => m.ActivityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Tag).WithMany(m => m.Activities).HasForeignKey(m => new { m.TagKey, m.TagValue }).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
