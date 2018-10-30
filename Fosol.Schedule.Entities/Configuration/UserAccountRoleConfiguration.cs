using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class UserAccountRoleConfiguration : IEntityTypeConfiguration<UserAccountRole>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserAccountRole> builder)
        {

            builder
                .ToTable("UserAccountRoles");

            builder
                .HasKey(m => new { m.UserId, m.AccountRoleId });
        }
        #endregion
    }
}
