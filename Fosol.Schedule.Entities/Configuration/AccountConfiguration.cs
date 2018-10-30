using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts"); // TODO: Add schema and make it DI configurable.
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Key).IsRequired();
            builder.Property(m => m.Email).HasMaxLength(150);
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.OwnsOne(m => m.BusinessAddress).Property(m => m.Name).HasMaxLength(100).HasColumnName("BusinessName");
            builder.OwnsOne(m => m.BusinessAddress).Property(m => m.Address1).HasMaxLength(150).HasColumnName("BusinessAddress1");
            builder.OwnsOne(m => m.BusinessAddress).Property(m => m.Address2).HasMaxLength(150).HasColumnName("BusinessAddress2");
            builder.OwnsOne(m => m.BusinessAddress).Property(m => m.City).HasMaxLength(150).HasColumnName("BusinessCity");
            builder.OwnsOne(m => m.BusinessAddress).Property(m => m.Province).HasMaxLength(150).HasColumnName("BusinessProvince");
            builder.OwnsOne(m => m.BusinessAddress).Property(m => m.Country).HasMaxLength(100).HasColumnName("BusinessCountry");
            builder.OwnsOne(m => m.BusinessAddress).Property(m => m.PostalCode).HasMaxLength(20).HasColumnName("BusinessPostalCode");

            builder.OwnsOne(m => m.FaxNumber).Property(m => m.Name).HasMaxLength(50).HasColumnName("FaxName");
            builder.OwnsOne(m => m.FaxNumber).Property(m => m.Number).HasMaxLength(25).HasColumnName("FaxNumber");

            builder.OwnsOne(m => m.TollFreeNumber).Property(m => m.Name).HasMaxLength(50).HasColumnName("TollFreeName");
            builder.OwnsOne(m => m.TollFreeNumber).Property(m => m.Number).HasMaxLength(25).HasColumnName("TollFreeNumber");

            builder.OwnsOne(m => m.BusinessPhone).Property(m => m.Name).HasMaxLength(50).HasColumnName("BusinessPhoneName");
            builder.OwnsOne(m => m.BusinessPhone).Property(m => m.Number).HasMaxLength(25).HasColumnName("BusinessPhone");

            builder.HasOne(m => m.Subscription).WithMany(m => m.Accounts).HasForeignKey(m => m.SubscriptionId).IsRequired().OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.Owner).WithMany(m => m.OwnedAccounts).HasForeignKey(m => m.OwnerId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.Key }).IsUnique();
            builder.HasIndex(m => new { m.OwnerId, m.State });
        }
        #endregion
    }
}
