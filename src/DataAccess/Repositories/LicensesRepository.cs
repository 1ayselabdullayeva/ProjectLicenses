using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using System.Linq;


namespace DataAccessLayer.Repositories
{
    public class LicensesRepository : Repository<Licenses>, ILicensesRepository
	{
		private readonly DbContext _dbContext;

		public LicensesRepository(DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
        public Licenses GetLicensesId(Licenses licensesId)
        {
            return FindByCondition(p => p.Id.Equals(licensesId))
                .DefaultIfEmpty(new Licenses())
                .FirstOrDefault();
        }

        public PagedList<Licenses> GetLicenses(PagedParameters productParameters)
        {
            return PagedList<Licenses>.ToPagedList(FindAll(),
            productParameters.PageNumber,
                 productParameters.PageSize);
        }

    }
}
