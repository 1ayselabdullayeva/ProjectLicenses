using Application.FluentValidation;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User.Create;
using Models.Entities;
using System.Security.Claims;

namespace Api.Controllers
{
    //[Authorize("Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("GetUser")]
        public IActionResult GetById() 
       {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response =_userServices.GetById(id);
            return Ok(response);
        }

        [HttpGet("GetLicenses")]
        public IActionResult GetByIdLicenses()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = _userServices.GetLicensesStatus(id);
            return Ok(response);

        }
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserCreateDto request)
        {
            UserValidator userValidator = new UserValidator();

            var validator = userValidator.Validate(new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                CompanyName = request.CompanyName
            }
            );
            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors);
            }
            var response = await _userServices.Create(request);
            return Ok(response);
        }
    }
}