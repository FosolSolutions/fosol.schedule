using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class OpeningParticipantApplicationConfiguration : IEntityTypeConfiguration<OpeningParticipantApplication>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<OpeningParticipantApplication> builder)
        {
            builder.ToTable("OpeningParticipantApplications");

            builder.HasKey(m => new { m.OpeningId, m.ParticipantId });

            builder.HasOne(m => m.Opening).WithMany(m => m.Applications).HasForeignKey(m => m.OpeningId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.Participant).WithMany(m => m.Applications).HasForeignKey(m => m.ParticipantId).OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
    }
}
