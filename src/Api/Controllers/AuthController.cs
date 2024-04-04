using Application.FluentValidation;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User.ForgotPassword;
using Models.DTOs.User.Login;
using Models.DTOs.User.Login.UserRefreshTokenDto;
using Models.DTOs.User.Register;
using Models.Entities;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJWTManagerRepository _jWTManager;
        private readonly IJWTServices _jWTServices;
 

        public AuthController(IJWTManagerRepository jWTManager, IJWTServices jWTServices)
        {
            _jWTManager = jWTManager;
            _jWTServices = jWTServices; 
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDto usersdata)
        {
           try
            {
                var tokens = _jWTServices.Login(usersdata);
                return Ok(tokens);
            }
            catch (Exception e)
            {
                return BadRequest("Istifadeci tapilmadi");
            }
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            UserValidator userValidator = new UserValidator();

            var validator = userValidator.Validate(new User
            {
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                Email = userRegister.Email,
                Password = userRegister.Password,
                PhoneNumber = userRegister.PhoneNumber,
                CompanyName = userRegister.CompanyName
            }
            );
            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors);
            }
            var response = await _jWTServices.Register(userRegister);
            return Ok(response);
        }
        [HttpPost]
        [Route("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _jWTServices.LogOut(userId);
            Response.Headers.Remove("Authorization");
            return Ok("Logout successful");
        }

        [HttpPost]
        [Route("get-token")]
        public ActionResult<List<UserAccessTokenDto>> GetTokenByRefreshToken(string refreshToken)
        {
            try
            {
                var userRefreshTokenDto = _jWTManager.GetTokenByRefreshToken(refreshToken);
                return Ok(userRefreshTokenDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error" });
            }
        }

        [HttpPost]
        [Route("Resetpassword")]
        public IActionResult ResetPassword(ResetPasswordDto request)
        {
            _jWTManager.ResetPassword(request);
            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto model)
        {
          
            await _jWTManager.ForgotPassword(model,Request.Headers["origin"]);
            return Ok();
        }

        [HttpPost("getIdfromToken")]
        public IActionResult GetIdFromToken(string token)
        {
            try
            {                
                var userId = _jWTManager.GetUserIdFromToken(token);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid Token" });
            }
        }

    }
}
