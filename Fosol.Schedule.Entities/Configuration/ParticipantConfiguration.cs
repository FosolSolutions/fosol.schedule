using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder
                .ToTable("Participants");

            builder
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(m => new { m.Key })
                .IsUnique();

            builder
                .HasIndex(m => new { m.CalendarId, m.DisplayName })
                .IsUnique();

            builder
                .HasIndex(m => new { m.Email, m.State });

            builder
                .HasOne(m => m.AddedBy)
                .WithMany()
                .HasForeignKey(m => m.AddedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(m => m.RowVersion)
                .IsRowVersion().IsConcurrencyToken();

            builder
                .HasOne(m => m.User)
                .WithMany(m => m.Participants)
                .HasForeignKey(m => m.UserId)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.Attributes)
                .WithOne(m => m.Participant)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasMany(m => m.ContactInfo)
                .WithOne(m => m.Participant)
                .OnDelete(DeleteBehavior.ClientSetNull);

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
