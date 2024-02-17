using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccessLayer.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.Property(p => p.FirstName).HasColumnType("nvarchar").HasMaxLength(25).IsRequired();
			builder.Property(p => p.LastName).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
			builder.Property(p => p.PhoneNumber).HasColumnType("nvarchar").HasMaxLength(20).IsRequired();
			builder.Property(p => p.Password).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
			builder.Property(p => p.Email).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
			builder.Property(p => p.Status);
			builder.Property(p => p.CompanyName).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
			builder.HasOne(p => p.Roles).WithMany(p => p.Users).HasForeignKey(p => p.RolesId)
			   .HasConstraintName("FK_Users_Role_Id");
			builder.HasOne(p => p.Licenses).WithMany(p => p.Users).HasForeignKey(p => p.LicensesId)
				 .HasConstraintName("FK_Users_Licenses_Id");
			builder.ToTable("User");

		}
	}
}
