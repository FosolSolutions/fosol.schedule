using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class CriteriaObjectConfiguration : IEntityTypeConfiguration<CriteriaObject>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<CriteriaObject> builder)
        {
            builder
                .ToTable("Criteria");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

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
