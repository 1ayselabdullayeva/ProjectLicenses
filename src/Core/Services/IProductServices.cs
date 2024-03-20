using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Product.Create;
using Models.DTOs.Product.Delete;
using Models.DTOs.Product.GetAll;
using Models.DTOs.Product.Update;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.GetById;

namespace Core.Services
{
	public interface IProductServices
	{
        public Task<ProductCreateResponseDto> Create(ProductCreateDto request);
        public ProductRemoveResponseDto Delete(int id);
        public ProductUpdateResponseDto Update(ProductUpdateDto productUpdateDto);
        public List<ProductGetAllResponseDto> GetAll();
        public List<ProductGetAllResponseDto> GetById(int id);
        public List<ProductGetAllResponseDto> GetProductPagingData([FromQuery] PagedParameters prodParam);
    }
}
