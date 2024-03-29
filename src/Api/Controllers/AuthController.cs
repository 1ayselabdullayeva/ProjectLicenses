﻿using Application.FluentValidation;
using Application.Services;
using Core.Exceptions;
using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User.Login;
using Models.DTOs.User.Login.AddRefreshToken;
using Models.DTOs.User.Login.UserRefreshTokenDto;
using Models.DTOs.User.Register;
using Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJWTManagerRepository _jWTManager;
        private readonly IJWTServices _jWTServices;
        private readonly IRolesServices _rolesService;
        private readonly IUserRepository _userRepository;

        public AuthController(IJWTManagerRepository jWTManager, IJWTServices jWTServices, IRolesServices rolesService, IUserRepository userRepository)
        {
            _jWTManager = jWTManager;
            _jWTServices = jWTServices; ;
            _rolesService = rolesService;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDto usersdata)
        {
            //try
            //{
            //    var user = _userRepository.GetAll(
            //    u => u.Email == usersdata.Email.ToLower()).FirstOrDefault();
            //    var sha = SHA256.Create();
            //    var asByteArray = Encoding.UTF8.GetBytes(usersdata.Password);
            //    var hasherPassword = sha.ComputeHash(asByteArray);
            //    var hashedPasswordString = Convert.ToBase64String(hasherPassword);
            //    if (user.Password != hashedPasswordString)
            //        throw new UserNotFoundException();
            //    var userRoles = _rolesService.GetRoleName(user.RolesId);

            //    var token = _jWTManager.GenerateJWTTokens(user.Id, user.FirstName, userRoles.RoleName,usersdata.RememberMe);

            //    if (token == null)
            //    {
            //        return Unauthorized("Invalid Attempt..");
            //    }

            //    User users = new User
            //    {
            //        Id = user.Id,
            //        RefreshToken = token.RefreshToken
            //    };

            //    _jWTServices.AddUserRefreshTokens(users);
            //    return Ok(token);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest("Istifadeci tapilmadi");
            //}
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
                FirstName= userRegister.FirstName,
                LastName= userRegister.LastName,
                Email= userRegister.Email,
                Password= userRegister.Password,
                PhoneNumber= userRegister.PhoneNumber,
                CompanyName= userRegister.CompanyName
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
    }
}
