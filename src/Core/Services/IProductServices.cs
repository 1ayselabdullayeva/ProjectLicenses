using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Product.Create;
using Models.DTOs.Product.Delete;
using Models.DTOs.Product.GetAll;
using Models.DTOs.Product.Update;

namespace Core.Services
{
    public interface IProductServices
	{
         Task<ProductCreateResponseDto> Create(ProductCreateDto request);
         ProductRemoveResponseDto Delete(int id);
         ProductUpdateResponseDto Update(ProductUpdateDto productUpdateDto);
         List<ProductGetAllResponseDto> GetAll();
         List<ProductGetAllResponseDto> GetById(int id);
         List<ProductGetAllResponseDto> GetProductPagingData([FromQuery] PagedParameters prodParam);
    }
}
