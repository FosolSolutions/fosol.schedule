using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class UserAttributeConfiguration : IEntityTypeConfiguration<UserAttribute>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserAttribute> builder)
        {
            builder.ToTable("UserAttributes");

            builder.HasKey(m => new { m.UserId, m.AttributeId });

            builder.HasOne(m => m.User).WithMany(m => m.Attributes).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Attribute).WithMany(m => m.Users).HasForeignKey(m => m.AttributeId).OnDelete(DeleteBehavior.Cascade);
        }
        #endregion
    }
}
