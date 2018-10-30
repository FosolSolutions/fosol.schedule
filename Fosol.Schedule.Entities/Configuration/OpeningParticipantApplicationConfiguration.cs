using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class OpeningParticipantApplicationConfiguration : IEntityTypeConfiguration<OpeningParticipantApplication>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningParticipantApplication> builder)
        {

            builder
                .ToTable("OpeningParticipantApplications");

            builder
                .HasKey(m => new { m.OpeningId, m.ParticipantId });
        }
        #endregion
    }
}
