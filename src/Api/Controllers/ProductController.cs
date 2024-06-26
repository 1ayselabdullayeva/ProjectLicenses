﻿using Application.FluentValidation;
using Core.Common.Utilities;
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


    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly IProductRepository _productRepository;
        private  readonly ILogger<ProductController> _logger;
    public ProductController(IProductServices productService, IProductRepository productRepository, ILogger<ProductController> logger)
    {
        _productService = productService;
        _productRepository = productRepository;
        _logger = logger;
    }
    

        [HttpGet("GetAllProducts")]
        [HasPermission("Products_Get_List")]
        public IActionResult GetAll()
        {
        _logger.LogError("GetAll Products started");
            var products = _productService.GetAll();
            return Ok(products);
        }


        [HttpGet("PaginationProduct")]
        [HasPermission("Products_Get_List")]

    public ActionResult<List<ProductGetAllResponseDto>> GetProductPagingData([FromQuery] PagedParameters prodParam,string sortBy)
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

        
        var responese = _productService.GetProductPagingData(prodParam);
        switch (sortBy)
        {
            case "name":
                responese = responese.OrderBy(p => p.ProductName).ToList();
                break;
            default:
                break;
        }
        return responese;
        }

        [HttpPost("Create")]
        [HasPermission("Product_Create")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto request)
        {
        ProductValidator pv= new ProductValidator();
        var validator = pv.Validate(new Product
        {
          ProductName = request.ProductName,
        });
        if (!validator.IsValid)
        {
            return BadRequest(validator);
        }
        var product = await _productService.Create(request);
            return Ok(product);

        }
        [HttpDelete("DeleteProduct")]
        [HasPermission("Product_Delete")]
    public IActionResult Delete(int id)
        {
            var product = _productService.Delete(id);
            return Ok(product);
        }
        [HttpPut("UpdateProduct")]
        [HasPermission("Product_Update")]
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

