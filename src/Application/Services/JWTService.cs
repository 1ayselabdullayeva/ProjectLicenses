using Application.Helper.Hasher;
using Business.Services;
using Core.Exceptions;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.User.ForgotPassword;
using Models.DTOs.User.Login;
using Models.DTOs.User.Register;
using Models.Entities;
using System.Security.Cryptography;
using System.Text;
namespace Application.Services
{
    public class JWTService : IJWTServices
    {   private readonly IUserRepository _userRepository;
        private readonly IRolesServices _rolesServices;
        private readonly IJWTManagerRepository _jwtManagerRepository;
        private readonly IEmailSenderServices _emailSenderServices;

        public JWTService(IUserRepository userRepository, IRolesServices rolesServices, IJWTManagerRepository jwtManagerRepository, IEmailSenderServices emailSenderServices)
        {
            _userRepository = userRepository;
            _rolesServices = rolesServices;
            _jwtManagerRepository = jwtManagerRepository;
            _emailSenderServices = emailSenderServices;
        }
        public void AddUserRefreshTokens(User user)
        {
            var existingUser = _userRepository.GetSingle(x=>x.Id==user.Id); 
            if (existingUser != null)
            {
                existingUser.RefreshToken = user.RefreshToken;
                _userRepository.Edit(existingUser);
            }
            else
            {
                _userRepository.Add(user);
            }
            _userRepository.Save();
        }
        public void DeleteUserRefreshTokens(int Id, string refreshToken)
        {
            var item = _userRepository.GetSingle(x => x.Id == Id && x.RefreshToken == refreshToken);
            if (item != null)
            {
                _userRepository.Remove(item);
                _userRepository.Save();
            }
        }

        public Tokens Login(UserLoginDto usersdata)
        {
            var user = _userRepository.GetAll(u => u.Email == usersdata.Email.ToLower()).FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException();

            var sha = SHA256.Create();
            var asByteArray = Encoding.UTF8.GetBytes(usersdata.Password);
            var hasherPassword = sha.ComputeHash(asByteArray);
            var hashedPasswordString = Convert.ToBase64String(hasherPassword);

            if (user.Password != hashedPasswordString)
                throw new UserNotFoundException();

            var userRoles = _rolesServices.GetRoleName(user.RolesId);

            var token = _jwtManagerRepository.GenerateJWTTokens(user.Id, user.FirstName, userRoles.RoleName, usersdata.RememberMe);
            var c = new Tokens
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
            };

            if (token == null)
                throw new Exception("Failed to generate JWT token");

            User users = new User
            {
                Id = user.Id,
                RefreshToken = token.RefreshToken
            };
             AddUserRefreshTokens(users);

            return token;
        }

    public void LogOut(int userId)
        {
            var userTokens = _userRepository.GetAll(u => u.Id == userId);

            foreach (var token in userTokens)
            {
              token.RefreshToken= null;
                _userRepository.Edit(token);
            }
             _userRepository.Save();
        }
        public async Task<UserRegisterResponseDto> Register(UserRegisterDto userRegister)
        
        {
            //var sha = SHA256.Create();
            //var asByteArray = Encoding.UTF8.GetBytes(userRegister.Password);
            //var hasherPassword = sha.ComputeHash(asByteArray);
            //var hashedPasswordString = Convert.ToBase64String(hasherPassword);
            var defaultRole = _rolesServices.GetDefaultRole();
            userRegister.RolesId = defaultRole.Id;
            var hashedPasswordString = PasswordHasherDto.Hasher(userRegister.Password);

            var userEntity = new User
                {
                    FirstName = userRegister.FirstName,
                    LastName = userRegister.LastName,
                    Email = userRegister.Email.ToLower(),
                    Password = hashedPasswordString,
                    CompanyName = userRegister.CompanyName,
                    RolesId = userRegister.RolesId,
                    PhoneNumber = userRegister.PhoneNumber
                };
                await _userRepository.AddAsync(userEntity);
            await _emailSenderServices.SendEmail(userEntity.Email, "User Registered", "Welcome to Aysel's web site");

            var userToReturn = new UserRegisterResponseDto
                {
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    Email = userEntity.Email,
                    CompanyName = userEntity.CompanyName,
                    RolesId = userEntity.RolesId,
                    PhoneNumber = userEntity.PhoneNumber
                };
                return userToReturn;
        }

        public async Task ResetPassword(ForgotPasswordDto request)
        {
             var user = _userRepository.GetSingle(x => x.Email == request.Email);
             if(user != null)
            {
                await _emailSenderServices.SendEmail(user.Email, "Salam", "Parolunuzu yenileyin");
            }

            
        }
    }
}

