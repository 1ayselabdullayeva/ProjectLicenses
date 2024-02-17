using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccessLayer.Configurations
{
	public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
	{
		public void Configure(EntityTypeBuilder<Ticket> builder)
		{

			builder.Property(p => p.Description).HasColumnType("nvarchar").HasMaxLength(1000).IsRequired();
			builder.HasOne(p => p.User).WithMany(p => p.Tickets).HasForeignKey(p => p.UserId)
				.HasConstraintName("FK_Ticket_UserId");
			builder.HasOne(p => p.Licenses).WithMany(p => p.Ticket).HasForeignKey(p => p.LicensesId)
				.HasConstraintName("FK_Ticket_Licenses_Id");
			builder.Property(p => p.Subject).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
			builder.Property(p => p.CreatedAt).HasColumnType("date");
			builder.ToTable("Ticket");

		}
	}
}