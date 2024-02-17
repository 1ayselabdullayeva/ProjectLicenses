using Microsoft.EntityFrameworkCore;
using Models.DTOs.User.Login;
using Models.DTOs.User.Register;
using Models.Entities;

namespace DataAccessLayer.Persistence
{
	public class ProjectDbContext : DbContext
	{
		public ProjectDbContext(DbContextOptions options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
		}
		public virtual DbSet<UserRefreshToken> UserRefreshToken { get; set; }

	}
}
