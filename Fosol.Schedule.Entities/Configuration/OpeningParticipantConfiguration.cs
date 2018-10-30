using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class OpeningParticipantConfiguration : IEntityTypeConfiguration<OpeningParticipant>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningParticipant> builder)
        {
            builder.ToTable("OpeningParticipants");

            builder.HasKey(m => new { m.OpeningId, m.ParticipantId });

            builder.HasOne(m => m.Opening).WithMany(m => m.Participants).HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Participant).WithMany(m => m.Openings).HasForeignKey(m => m.ParticipantId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
