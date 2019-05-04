﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fosol.Schedule.Entities.Configuration
{
	public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
	{
		#region Methods
		public void Configure(EntityTypeBuilder<Attribute> builder)
		{
			builder.ToTable("Attributes");

			builder.HasKey(m => m.Id);

			builder.Property(m => m.Id).ValueGeneratedOnAdd();
			builder.Property(m => m.Key).HasMaxLength(100).IsRequired();
			builder.Property(m => m.Value).HasMaxLength(100).IsRequired();
			builder.Property(m => m.ValueType).HasMaxLength(200).IsRequired();
			builder.Property(m => m.RowVersion).IsRowVersion();

			builder.HasOne(m => m.Calendar).WithMany(m => m.Attributes).HasForeignKey(m => m.CalendarId).OnDelete(DeleteBehavior.ClientSetNull);
			builder.HasOne(m => m.AddedBy).WithMany().HasForeignKey(m => m.AddedById).OnDelete(DeleteBehavior.ClientSetNull);
			builder.HasOne(m => m.UpdatedBy).WithMany().HasForeignKey(m => m.UpdatedById).OnDelete(DeleteBehavior.ClientSetNull);

			builder.HasIndex(m => new { m.CalendarId, m.Key, m.Value }).IsUnique(true);
		}
		#endregion
	}
}
