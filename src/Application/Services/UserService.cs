using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Repositories;
using Models.DTOs.Product.Create;
using Models.DTOs.Tickets.GetById;
using Models.DTOs.User.Create;
using Models.DTOs.User.GetById;
using Models.Entities;

namespace Business.Services
{
	public class UserService : IUserServices
	{
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        public UserService(IUserRepository userRepository, IRolesRepository rolesRepository)
        {
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
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
                PhoneNumber = request.PhoneNumber
            };
            await _userRepository.AddAsync(newProduct);
            var responseDto = new UserCreateResponseDto
            {
                FirstName= newProduct.FirstName,
                LastName= newProduct.LastName,
                Email= newProduct.Email,
                Password= newProduct.Password,
                CompanyName= newProduct.CompanyName,
                PhoneNumber= newProduct.PhoneNumber
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
                Email= user.Email
            };
            return response;
        }
    }
}
