using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class OpeningCriteriaConfiguration : IEntityTypeConfiguration<OpeningCriteria>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningCriteria> builder)
        {

            builder
                .ToTable("OpeningCriteria");

            builder
                .HasKey(m => new { m.OpeningId, m.CriteriaId });
        }
        #endregion
    }
}
