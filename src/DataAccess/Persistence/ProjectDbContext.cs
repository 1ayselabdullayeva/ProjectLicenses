﻿using Microsoft.EntityFrameworkCore;

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
		

	}
}
