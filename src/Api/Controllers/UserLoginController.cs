using Application.Services;
using Core.Exceptions;
using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User.Login;
using Models.DTOs.User.Register;
using Models.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IJWTManagerRepository _jWTManager;
        private readonly IJWTServices _jWTServices;
        private readonly IRolesServices _rolesService;
        private readonly IUserRepository _userRepository;

        public UserLoginController(IJWTManagerRepository jWTManager, IJWTServices jWTServices, IRolesServices rolesService, IUserRepository userRepository)
        {
            _jWTManager = jWTManager;
            _jWTServices = jWTServices; ;
            _rolesService = rolesService;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate-user")]
        public async Task<IActionResult> AuthenticateAsync(UserLoginDto usersdata)
        {
            var user = _userRepository.GetAll(
            u => u.Email == usersdata.Email.ToLower()).FirstOrDefault();
            var sha = SHA256.Create();
            var asByteArray = Encoding.UTF8.GetBytes(usersdata.Password);
            var hasherPassword = sha.ComputeHash(asByteArray);
            var hashedPasswordString = Convert.ToBase64String(hasherPassword);
            if (user.Password != hashedPasswordString)
                throw new UserNotFoundException();
            var userRoles = _rolesService.GetRoleName(user.RolesId);
            var validUser = await _jWTServices.IsValidUserAsync(usersdata);
            
            if (!validUser)
            {
                return Unauthorized("Invalid username or password...");
            }

            var token = _jWTManager.GenerateToken(user.Id ,user.FirstName, userRoles.RoleName);

            if (token == null)
            {
                return Unauthorized("Invalid Attempt..");
            }

            UserRefreshToken users = new UserRefreshToken
            {
                RefreshToken = token.RefreshToken,
                UserId = user.Id
            };

            _jWTServices.AddUserRefreshTokens(users);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public IActionResult Refresh(Tokens token)
        {
            var principal = _jWTManager.GetPrincipalFromExpiredToken(token.AccessToken);
            var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            var name = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            var id = int.Parse(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var savedRefreshToken = _jWTServices.GetSavedRefreshTokens(id, token.RefreshToken);
            if (savedRefreshToken.RefreshToken != token.RefreshToken)
            {
                return Unauthorized("Invalid attempt!");
            }
           
            var newJwtToken = _jWTManager.GenerateRefreshToken(id,name, roleClaim);

            if (newJwtToken == null)
            {
                return Unauthorized("Invalid attempt!");
            }

            UserRefreshToken obj = new UserRefreshToken
            {
                RefreshToken = newJwtToken.RefreshToken,
                UserId = id
            };

            _jWTServices.DeleteUserRefreshTokens(id, token.RefreshToken);
            _jWTServices.AddUserRefreshTokens(obj);

            return Ok(newJwtToken);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
        {
            var response = await _jWTServices.Register(userRegister);
            return Ok(response);
        }

    }
}
