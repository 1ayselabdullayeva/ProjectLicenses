using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccessLayer.Repositories
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly DbContext _dbContext;

		public UserRepository(DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
    }

}
