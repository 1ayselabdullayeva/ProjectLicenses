using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccessLayer.Repositories
{
	public class RolesRepository : Repository<Roles>, IRolesRepository
	{
		private readonly DbContext _dbContext;

		public RolesRepository(DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
