using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class UserContactInfoConfiguration : IEntityTypeConfiguration<UserContactInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserContactInfo> builder)
        {

            builder
                .ToTable("UserContactInfo");

            builder
                .HasKey(m => new { m.UserId, m.ContactInfoId });
        }
        #endregion
    }
}
