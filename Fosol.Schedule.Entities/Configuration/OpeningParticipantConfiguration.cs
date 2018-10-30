using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class OpeningParticipantConfiguration : IEntityTypeConfiguration<OpeningParticipant>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningParticipant> builder)
        {

            builder
                .ToTable("OpeningParticipants");

            builder
                .HasKey(m => new { m.OpeningId, m.ParticipantId });
        }
        #endregion
    }
}
