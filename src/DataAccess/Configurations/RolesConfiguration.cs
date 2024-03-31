using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccessLayer.Configurations
{
	public class RolesConfiguration : IEntityTypeConfiguration<Roles>
	{
		public void Configure(EntityTypeBuilder<Roles> builder)
		{
			builder.Property(p => p.RoleName).HasColumnType("VARCHAR(25)");
			builder.ToTable("Roles");
		}

	}
}
