using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class UserAttributeConfiguration : IEntityTypeConfiguration<UserAttribute>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserAttribute> builder)
        {

            builder
                .ToTable("UserAttributes");

            builder
                .HasKey(m => new { m.UserId, m.AttributeId });
        }
        #endregion
    }
}
