using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.Entities;


namespace DataAccessLayer.Repositories
{
	public class LicensesRepository : Repository<Licenses>, ILicensesRepository
	{
		private readonly DbContext _dbContext;

		public LicensesRepository(DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
