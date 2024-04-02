using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Models.DTOs.Licenses.GetById;
using Models.DTOs.Product.Create;
using Models.DTOs.Tickets.GetById;
using Models.DTOs.User.Create;
using Models.DTOs.User.GetById;
using Models.DTOs.User.GetLicenses;
using Models.Entities;
using Models.Enums;
using System.ComponentModel;
using System.Numerics;

namespace Business.Services
{
	public class UserService : IUserServices
	{
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly ILicensesRepository _licenseRepository;
        public UserService(IUserRepository userRepository, IRolesRepository rolesRepository, ILicensesRepository licenseRepository)
        {
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            _licenseRepository = licenseRepository;
        }

        public async Task<UserCreateResponseDto> Create(UserCreateDto request)
        {
            var newProduct = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                CompanyName = request.CompanyName,
                PhoneNumber = request.PhoneNumber,
                RolesId = request.RolesId,
            };
            await _userRepository.AddAsync(newProduct);
            var responseDto = new UserCreateResponseDto
            {
                FirstName= newProduct.FirstName,
                LastName= newProduct.LastName,
                Email= newProduct.Email,
                Password= newProduct.Password,
                CompanyName= newProduct.CompanyName,
                PhoneNumber= newProduct.PhoneNumber,
                RolesId = request.RolesId,
            };
            return responseDto;
        }

        public UserGetByIdResponseDto GetById(int id)
        {
            var user = _userRepository.GetSingle(m => m.Id == id);
            var response = new UserGetByIdResponseDto
            {
                FirstName= user.FirstName, 
                LastName= user.LastName,
                CompanyName = user.CompanyName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return response;
        }
        public UserGetLicensesResponseDto GetLicensesStatus(int id)
        {
            var users=_userRepository.GetSingle(x=>x.Id==id);
            var lisences = _licenseRepository.GetAll(u => u.UserId == id);
            var activeCount = 0;
            var notActiveCount = 0;
            var expiredCount = 0;

            foreach(var lisence in lisences)
            {
                switch (lisence.LicenseStatus)
                {
                    case LiscenseStatus.Active:
                        activeCount++;
                        break;
                    case LiscenseStatus.NotActivated:
                        notActiveCount++;
                        break;
                    case LiscenseStatus.Expired:
                        expiredCount++;
                        break;
                    default:
                        break;
                }
            }
            var totalLicenses = activeCount + notActiveCount + expiredCount;
            var responseDto = new UserGetLicensesResponseDto
            {
                CompanyName = users.CompanyName,
                ActiveLicensesCount = activeCount,
                LockedLicensesCount = notActiveCount,
                ExpiredLicensesCount = expiredCount,
                TotalLicenses = totalLicenses
            };
            return responseDto;

        }
           
    }
}
