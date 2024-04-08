using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccess.Repositories
{
    public class PermissionsRepository : Repository<Permissions>, IPermissionsRepository
    {
        private readonly DbContext _dbContext;

        public PermissionsRepository(DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
