using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ParticipantContactInfoConfiguration : IEntityTypeConfiguration<ParticipantContactInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<ParticipantContactInfo> builder)
        {

            builder
                .ToTable("ParticipantContactInfo");

            builder
                .HasKey(m => new { m.ParticipantId, m.ContactInfoId });
        }
        #endregion
    }
}
