using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccessLayer.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.Property(p => p.FirstName).HasColumnType("VARCHAR(25)").IsRequired();
			builder.Property(p => p.LastName).HasColumnType("VARCHAR(50)").IsRequired();
			builder.Property(p => p.PhoneNumber).HasColumnType("VARCHAR(20)").IsRequired();
			builder.Property(p => p.Password).HasColumnType("VARCHAR(50)").IsRequired();
			builder.Property(p => p.Email).HasColumnType("VARCHAR(50)").IsRequired();
			builder.Property(p => p.Status);
			builder.Property(p => p.CompanyName).HasColumnType("VARCHAR(50)").IsRequired();
			builder.HasOne(p => p.Roles).WithMany(p => p.Users).HasForeignKey(p => p.RolesId)
			   .HasConstraintName("FK_Users_Role_Id");
			//builder.HasOne(p => p.Licenses).WithMany(p => p.Users).HasForeignKey(p => p.LicensesId)
			//	 .HasConstraintName("FK_Users_Licenses_Id");
			builder.ToTable("User");

		}
	}
}
