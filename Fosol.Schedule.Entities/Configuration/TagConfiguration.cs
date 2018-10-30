using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");

            builder.HasKey(m => new { m.Key, m.Value });

            builder.Property(m => m.Key).HasMaxLength(50).IsRequired();
            builder.Property(m => m.Value).HasMaxLength(100).IsRequired();
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
