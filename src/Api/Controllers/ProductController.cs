using Application.FluentValidation;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Product.Create;
using Models.DTOs.Product.Update;
using Models.Entities;

namespace Api.Controllers
{
    [Authorize("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        public ProductController(IProductServices productService)
        {
            _productService = productService;
        }
        
        [HttpGet("GetAllProducts")]
        public IActionResult GetAll()
        {
            //var text = HttpContext.User;
            var products = _productService.GetAll();
            return Ok(products);
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
