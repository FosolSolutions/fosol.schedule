using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("UserInfo");

            builder.HasKey(m => m.UserId);

            builder.Property(m => m.UserId).ValueGeneratedNever();
            builder.Property(m => m.Title).HasMaxLength(100);
            builder.Property(m => m.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(m => m.MiddleName).HasMaxLength(100);
            builder.Property(m => m.LastName).HasMaxLength(100).IsRequired();
            builder.Property(m => m.Description).HasMaxLength(1000);
            builder.Property(m => m.RowVersion).IsRowVersion();

            builder.OwnsOne(m => m.HomeAddress).Property(m => m.Name).HasMaxLength(100).HasColumnName("HomeName");
            builder.OwnsOne(m => m.HomeAddress).Property(m => m.Address1).HasMaxLength(150).HasColumnName("HomeAddress1");
            builder.OwnsOne(m => m.HomeAddress).Property(m => m.Address2).HasMaxLength(150).HasColumnName("HomeAddress2");
            builder.OwnsOne(m => m.HomeAddress).Property(m => m.City).HasMaxLength(150).HasColumnName("HomeCity");
            builder.OwnsOne(m => m.HomeAddress).Property(m => m.Province).HasMaxLength(150).HasColumnName("HomeProvince");
            builder.OwnsOne(m => m.HomeAddress).Property(m => m.Country).HasMaxLength(100).HasColumnName("HomeCountry");
            builder.OwnsOne(m => m.HomeAddress).Property(m => m.PostalCode).HasMaxLength(20).HasColumnName("HomePostalCode");

            builder.OwnsOne(m => m.WorkAddress).Property(m => m.Name).HasMaxLength(100).HasColumnName("WorkName");
            builder.OwnsOne(m => m.WorkAddress).Property(m => m.Address1).HasMaxLength(150).HasColumnName("WorkAddress1");
            builder.OwnsOne(m => m.WorkAddress).Property(m => m.Address2).HasMaxLength(150).HasColumnName("WorkAddress2");
            builder.OwnsOne(m => m.WorkAddress).Property(m => m.City).HasMaxLength(150).HasColumnName("WorkCity");
            builder.OwnsOne(m => m.WorkAddress).Property(m => m.Province).HasMaxLength(150).HasColumnName("WorkProvince");
            builder.OwnsOne(m => m.WorkAddress).Property(m => m.Country).HasMaxLength(100).HasColumnName("WorkCountry");
            builder.OwnsOne(m => m.WorkAddress).Property(m => m.PostalCode).HasMaxLength(20).HasColumnName("WorkPostalCode");

            builder.OwnsOne(m => m.HomePhone).Property(m => m.Name).HasMaxLength(50).HasColumnName("HomePhoneName");
            builder.OwnsOne(m => m.HomePhone).Property(m => m.Number).HasMaxLength(25).HasColumnName("HomePhone");

            builder.OwnsOne(m => m.MobilePhone).Property(m => m.Name).HasMaxLength(50).HasColumnName("MobilePhoneName");
            builder.OwnsOne(m => m.MobilePhone).Property(m => m.Number).HasMaxLength(25).HasColumnName("MobilePhone");

            builder.OwnsOne(m => m.WorkPhone).Property(m => m.Name).HasMaxLength(50).HasColumnName("WorkPhoneName");
            builder.OwnsOne(m => m.WorkPhone).Property(m => m.Number).HasMaxLength(25).HasColumnName("WorkPhone");

            builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(m => new { m.LastName, m.FirstName, m.Gender });
        }
        #endregion
    }
}
