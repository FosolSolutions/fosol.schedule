using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<AccountRole> builder)
        {
            builder.HasKey(m => m.Id);
            builder.ToTable("AccountRoles");
        }
        #endregion
    }
}
