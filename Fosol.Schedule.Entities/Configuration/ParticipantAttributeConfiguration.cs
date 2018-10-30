using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ParticipantAttributeConfiguration : IEntityTypeConfiguration<ParticipantAttribute>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ParticipantAttribute> builder)
        {

            builder
                .ToTable("ParticipantAttributes");

            builder
                .HasKey(m => new { m.ParticipantId, m.AttributeId });
        }
        #endregion
    }
}
