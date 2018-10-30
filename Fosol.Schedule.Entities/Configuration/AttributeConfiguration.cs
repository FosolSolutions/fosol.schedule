using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Attribute> builder)
        {
            builder
                .ToTable("Attributes");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Key, m.Value })
                .IsUnique(false);

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
