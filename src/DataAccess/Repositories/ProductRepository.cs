using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;
using System.Security.Cryptography;

namespace DataAccessLayer.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private readonly DbContext _dbContext;

		public ProductRepository(DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

        public Product GetProductId(Product productId)
        {
            return FindByCondition(p => p.Id.Equals(productId))
                .DefaultIfEmpty(new Product())
                .FirstOrDefault();
        }

        public PagedList<Product> GetProducts(PagedParameters productParameters)
        {
            return PagedList<Product>.ToPagedList(FindAll(),
            productParameters.PageNumber,
                 productParameters.PageSize);
        }
    }
}
