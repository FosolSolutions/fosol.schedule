using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ContactInfoConfiguration : IEntityTypeConfiguration<ContactInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ContactInfo> builder)
        {
            builder
                .ToTable("ContactInfo");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Name, m.Category, m.Value });

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
