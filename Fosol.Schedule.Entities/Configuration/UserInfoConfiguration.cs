using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder
                .ToTable("UserInfo");

            builder
                .Property(m => m.UserId)
                .ValueGeneratedNever();

            builder.OwnsOne(m => m.HomeAddress)
                .Property(m => m.Name)
                .HasColumnName("HomeName");

            builder.OwnsOne(m => m.HomeAddress)
                .Property(m => m.Address1)
                .HasColumnName("HomeAddress1");

            builder.OwnsOne(m => m.HomeAddress)
                .Property(m => m.Address2)
                .HasColumnName("HomeAddress2");

            builder.OwnsOne(m => m.HomeAddress)
                .Property(m => m.City)
                .HasColumnName("HomeCity");

            builder.OwnsOne(m => m.HomeAddress)
                .Property(m => m.Province)
                .HasColumnName("HomeProvince");

            builder.OwnsOne(m => m.HomeAddress)
                .Property(m => m.Country)
                .HasColumnName("HomeCountry");

            builder.OwnsOne(m => m.HomeAddress)
                .Property(m => m.PostalCode)
                .HasColumnName("HomePostalCode");


            builder.OwnsOne(m => m.WorkAddress)
                .Property(m => m.Name)
                .HasColumnName("WorkName");

            builder.OwnsOne(m => m.WorkAddress)
                .Property(m => m.Address1)
                .HasColumnName("WorkAddress1");

            builder.OwnsOne(m => m.WorkAddress)
                .Property(m => m.Address2)
                .HasColumnName("WorkAddress2");

            builder.OwnsOne(m => m.WorkAddress)
                .Property(m => m.City)
                .HasColumnName("WorkCity");

            builder.OwnsOne(m => m.WorkAddress)
                .Property(m => m.Province)
                .HasColumnName("WorkProvince");

            builder.OwnsOne(m => m.WorkAddress)
                .Property(m => m.Country)
                .HasColumnName("WorkCountry");

            builder.OwnsOne(m => m.WorkAddress)
                .Property(m => m.PostalCode)
                .HasColumnName("WorkPostalCode");
        }
        #endregion
    }
}
