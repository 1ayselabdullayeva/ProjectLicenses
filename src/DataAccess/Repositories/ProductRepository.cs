using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccessLayer.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private readonly DbContext _dbContext;

		public ProductRepository(DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
