using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder
                .ToTable("Tags");

            builder
                .HasKey(m => new { m.Key, m.Value });

            builder
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
