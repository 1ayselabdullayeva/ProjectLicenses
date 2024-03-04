using Business.Services;
using Core.Exceptions;
using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Persistence;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Models.DTOs.User.Login;
using Models.DTOs.User.Register;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JWTService : IJWTServices
    {      private readonly IUserRepository _userRepository;
        private readonly IRolesServices _rolesServices;

        public JWTService( IUserRepository userRepository, IRolesServices rolesServices)
        {
          
            _userRepository = userRepository;
            _rolesServices = rolesServices;
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
        var sha = SHA256.Create();
        var asByteArray = Encoding.UTF8.GetBytes(userRegister.Password);
        var hasherPassword = sha.ComputeHash(asByteArray);
        var hashedPasswordString = Convert.ToBase64String(hasherPassword);
        var defaultRole = _rolesServices.GetDefaultRole();
        userRegister.RolesId = defaultRole.Id;
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
}
}

