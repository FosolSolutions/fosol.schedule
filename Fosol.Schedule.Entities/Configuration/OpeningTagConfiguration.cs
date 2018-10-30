using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class OpeningTagConfiguration : IEntityTypeConfiguration<OpeningTag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningTag> builder)
        {
            builder.ToTable("OpeningTags");

            builder.HasKey(m => new { m.OpeningId, m.TagKey, m.TagValue });

            builder.HasOne(m => m.Opening).WithMany(m => m.Tags).HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Tag).WithMany(m => m.Openings).HasForeignKey(m => new { m.TagKey, m.TagValue }).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
