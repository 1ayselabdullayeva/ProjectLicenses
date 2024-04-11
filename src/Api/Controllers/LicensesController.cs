using Application.FluentValidation;
using Core.Common.Utilities;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Licenses.Create;
using Models.DTOs.Licenses.Update;
using Models.DTOs.Product.GetAll;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;

namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private ILicensesServices _licensesServices;
        private ILicensesRepository _licensesRepository;
        public LicensesController(ILicensesServices licensesServices,  ILicensesRepository licensesRepository)
        {
            _licensesServices = licensesServices;
            _licensesRepository = licensesRepository;
        }


        [HttpGet("GetAllLicenses")]
        [HasPermission("Licenses_Get_List")]

        public IActionResult GetAllLicenses()
        {
            var response = _licensesServices.GetAll();
            return Ok(response);
        }

        [HttpGet("PaginationLicenses")]
        [HasPermission("Licenses_Get_List")]

        public ActionResult<List<ProductGetAllResponseDto>> GetProductPagingData([FromQuery] PagedParameters prodParam,string sortBy)
        {
            var tickets = _licensesRepository.GetLicenses(prodParam);

            var metadata = new
            {
                tickets.TotalCount,
                tickets.PageSize,
                tickets.CurrentPage,
                tickets.TotalPages,
                tickets.HasNext,
                tickets.HasPrevious
            };

            HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var response = _licensesServices.GetLicensesPagingData(prodParam);
            switch (sortBy)
            {
                case "status":
                    response = response.OrderBy(p => p.licenseStatus).ToList();
                    break;
                case "Expire":
                    response = response.OrderBy(p => p.ExpireDate).ToList();
                    break;
                default:
                    break;
            }
            return Ok(response);
        }

        [HttpGet("getLicenses")]
        [HasPermission("License_GetById_list")]
        public IActionResult GetLicenses()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var responseDto = _licensesServices.GetById(id);
            return Ok(responseDto);
        }
        
        [HttpPost("Buy")]
        [HasPermission("License_Create")]
        public async Task<IActionResult>CreateLicenses(LicensesCreateDto request)
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            LicensesValidator lv = new LicensesValidator();
            var validator = lv.Validate(new Models.Entities.Licenses
            {
                ProductId=request.ProductId,
                UserCount=request.UserCount,
                ExpireDate = DateTime.Now.AddYears(request.Year)
            });
            if (!validator.IsValid)
            {
                return BadRequest(validator);
            }
            var response =await _licensesServices.CreateLicenses(id, request);
            return Ok(response);
        }
        [HttpGet("GetByIdLicenses")]
        [HasPermission("License_Get_Detal_List")]
        public IActionResult GetById(int LicensesId)
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response =_licensesServices.GetByIdLicenses(id, LicensesId);
            return Ok(response);
        }

        [HttpGet("types")]
        [HasPermission("License_Get_Status_List")]
        public IActionResult GetLicenseStatus()
        {
            var types = _licensesServices.GetLicensesStatus();
            return Ok(types);
        }

        [HttpPut("UpdateStatus")]
        [HasPermission("License_Update_Status")]
        public IActionResult Edit(LicensesUpdateStatusDto request)
        {
            var response = _licensesServices.Edit(request);
            return Ok(response);    
        }
    }
}
