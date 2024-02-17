using Business.Services;
using Core.Exceptions;
using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Persistence;
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
    {
        private readonly ProjectDbContext _db;
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRolesServices _rolesServices;

        public JWTService(ProjectDbContext db, IUserRepository userRepository, IRolesRepository rolesRepository, IRolesServices rolesServices)
        {
            _db = db;
        _userRepository = userRepository;
        _rolesRepository = rolesRepository;
        _rolesServices = rolesServices;
    }


    public UserRefreshToken AddUserRefreshTokens(UserRefreshToken user)
    {     

        _db.UserRefreshToken.Add(user);
        _db.SaveChanges();
        return user;
    }

    public void DeleteUserRefreshTokens(int Id, string refreshToken)
    {
        var item = _db.UserRefreshToken.FirstOrDefault(x => x.UserId == Id && x.RefreshToken == refreshToken);
        if (item != null)
        {
            _db.UserRefreshToken.Remove(item);
            _db.SaveChanges();
        }
    }

    public UserRefreshToken GetSavedRefreshTokens(int Id, string refreshtoken)
    {
        var item = _db.UserRefreshToken.FirstOrDefault(x => x.UserId == Id && x.RefreshToken == refreshtoken && x.IsActive == true);
        return item;
    }

    public async Task<bool> IsValidUserAsync(UserLoginDto users)
    {
        var sha = SHA256.Create();
        var asByteArray = Encoding.UTF8.GetBytes(users.Password);
        var hasherPassword = sha.ComputeHash(asByteArray);
        var hashedPasswordString = Convert.ToBase64String(hasherPassword);
        var user = _userRepository.GetAll(
        u => u.Email == users.Email.ToLower()).FirstOrDefault();
        if (user.Password != hashedPasswordString)
            throw new UserNotFoundException();
        if (user != null)
        {
            return true;
        }
        else
            return false;
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

