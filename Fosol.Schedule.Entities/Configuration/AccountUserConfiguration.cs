using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class AccountUserConfiguration : IEntityTypeConfiguration<AccountUser>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<AccountUser> builder)
        {
            builder
                .ToTable("AccountUsers");

            builder
                .HasKey(m => new { m.AccountId, m.UserId });
        }
        #endregion
    }
}
