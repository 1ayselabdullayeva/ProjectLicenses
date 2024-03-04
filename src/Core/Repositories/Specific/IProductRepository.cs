using Models.DTOs;
using Models.Entities;

namespace Core.Repositories.Specific
{
	public interface IProductRepository : IRepository<Product>
	{
        public PagedList<Product> GetProducts(PagedParameters productParameters);
        Product GetProductId(Product productId);
    }
}
