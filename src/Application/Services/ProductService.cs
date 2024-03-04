using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Product.Create;
using Models.DTOs.Product.Delete;
using Models.DTOs.Product.GetAll;
using Models.DTOs.Product.Update;
using Models.Entities;

namespace Business.Services
{
    public class ProductService : IProductServices
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductCreateResponseDto> Create(ProductCreateDto request)
        {
            var newProduct = new Product
            {
                ProductName = request.ProductName,
            };
            await _productRepository.AddAsync(newProduct);
            var responseDto = new ProductCreateResponseDto
            {
                ProductName = newProduct.ProductName
            };
            return responseDto;
        }
        public ProductRemoveResponseDto Delete(int id)
        {
           var product=_productRepository.GetSingle(m=>m.Id==id);
            _productRepository.Remove(product);
            _productRepository.Save();
            var response = new ProductRemoveResponseDto
            {
                ProductName = product.ProductName,
            };
            return response;
        }
        public List<ProductGetAllResponseDto> GetAll()
        {
            var product = _productRepository.GetAll();
            var products = product.Select(p => new ProductGetAllResponseDto
            {
                Id = p.Id,
                ProductName = p.ProductName
            }).ToList();
            return products;
        }
        public List<ProductGetAllResponseDto> GetProductPagingData([FromQuery] PagedParameters prodParam)
        {
            var products = _productRepository.GetProducts(prodParam);
                var responseDtoList = products.Select(p => new ProductGetAllResponseDto
                {
                     Id = p.Id,
                    ProductName = p.ProductName
                }).ToList();
                return responseDtoList;
        }
        public ProductUpdateResponseDto Update(ProductUpdateDto productUpdateDto)
        {
            var product = _productRepository.GetSingle(m => m.Id == productUpdateDto.Id);
            product.ProductName = productUpdateDto.ProductName;
            _productRepository.Edit(product);
            _productRepository.Save();
            var response =new ProductUpdateResponseDto
            {
                ProductName = product.ProductName,
            };
            return response;
        }
    }
}
