using Application.FluentValidation;
using Application.Services;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Product.Create;
using Models.DTOs.Product.GetAll;
using Models.DTOs.Product.Update;
using Models.Entities;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Authorize("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly IProductRepository _productRepository;
        public ProductController(IProductServices productService, IProductRepository productRepository)
        {
            _productService = productService;
            _productRepository = productRepository;
        }

        [HttpGet("GetAllProducts")]
        public IActionResult GetAll()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }
        [HttpGet("PaginationProduct")]
        public ActionResult<List<ProductGetAllResponseDto>> GetProductPagingData([FromQuery] PagedParameters prodParam)
        {
            var products = _productRepository.GetProducts(prodParam);

            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };

            HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var responseDtoList = products.Select(p => new ProductGetAllResponseDto
            {
                Id = p.Id,
                ProductName = p.ProductName

            }).ToList();
            return responseDtoList;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _productService.Create(request);
            return Ok(product);

        }
        [HttpDelete("DeleteProduct")]
        public IActionResult Delete(int id)
        {
            var product = _productService.Delete(id);
            return Ok(product);
        }
        [HttpPut("UpdateProduct")]
        public IActionResult Update([FromBody]ProductUpdateDto request)
        {
            ProductValidator pv = new ProductValidator();
            var validationResult = pv.Validate(new Product
            {
                ProductName = request.ProductName
            });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var product = _productService.Update(request);
          
            return Ok(product);
        }

        

    }
}
