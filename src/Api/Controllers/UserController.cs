using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize("Customer")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("GetUser")]

        [Authorize("Customer")]
        public IActionResult GetById() 
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response =_userServices.GetById(id);
            return Ok(response);
        }

        [HttpGet("GetLicenses")]

        [Authorize("Customer")]
        public IActionResult GetByIdLicenses()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = _userServices.GetLicensesStatus(id);
            return Ok(response);

        }
    }
}