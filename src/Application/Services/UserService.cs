using Core.Exceptions;
using Core.Repositories.Specific;
using Core.Services;
using Models.DTOs.User.Create;
using Models.DTOs.User.GetById;
using Models.DTOs.User.GetLicenses;
using Models.DTOs.User.Update;
using Models.Entities;
using Models.Enums;

namespace Business.Services
{
    public class UserService : IUserServices
	{
        private readonly IUserRepository _userRepository;
        private readonly ILicensesRepository _licenseRepository;
        public UserService(IUserRepository userRepository, ILicensesRepository licenseRepository)
        {
            _userRepository = userRepository;
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

        public UserUpdateResponseDto UserEdit(int id, UserUpdateDto request)
        {
            var user = _userRepository.GetSingle(x=>x.Id == id);
            if(user == null)
            {
                throw new ResourceNotFoundException();
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.CompanyName =  request.CompanyName;
            _userRepository.Edit(user);
            _userRepository.Save();
            var response = new UserUpdateResponseDto
            {
                CompanyName = user.CompanyName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return response;

        }
    }
}
