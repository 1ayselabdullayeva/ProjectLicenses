using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using System.ComponentModel;

namespace DataAccessLayer.Configurations
{
	public class LicensesConfiguration : IEntityTypeConfiguration<Licenses>
	{
		public void Configure(EntityTypeBuilder<Licenses> builder)
		{
			builder.Property(p => p.ExpireDate).HasColumnType("DATE");
			builder.Property(p => p.ActivationDate).HasColumnType("DATE");
			builder.HasOne(p=>p.Product).WithMany(p => p.Licenses).HasForeignKey(p => p.ProductId)
				.HasConstraintName("FK_Licenses_Product_Id");
            builder.HasOne(p => p.User).WithMany(p => p.Licenses).HasForeignKey(p => p.UserId)
                .HasConstraintName("FK_Licenses_User_Id");
            builder.ToTable("Licenses");
		}
	}
}

