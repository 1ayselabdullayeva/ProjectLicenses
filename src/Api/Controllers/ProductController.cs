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
using System.Security.Claims;

{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductServices productService, IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productService = productService;
            _productRepository = productRepository;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpGet("GetAllProducts")]
        public IActionResult GetAll()
        {
            _logger.LogDebug("GetAll Products");
            var products = _productService.GetAll();
            return Ok(products);
        }
        [HttpGet("GetById")]

        public IActionResult GetById()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var products = _productService.GetById(userId);
            return Ok(products);
        }
        [HttpGet("PaginationProduct")]
        [Authorize("Admin")]

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
        [Authorize("Admin")]
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
        [Authorize("Admin")]
        public IActionResult Delete(int id)
        {
            var product = _productService.Delete(id);
            return Ok(product);
        }
        [HttpPut("UpdateProduct")]
        [Authorize("Admin")]
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
