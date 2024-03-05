using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.GetById;
using Models.DTOs.User.Create;
using Models.DTOs.User.GetById;
using Models.DTOs.User.GetLicenses;

namespace Core.Services
{
	public interface IUserServices
	{
		public Task<UserCreateResponseDto> Create(UserCreateDto request);
        public UserGetByIdResponseDto GetById(int id);
        //public UserGetLicensesResponseDto GetLicensesStatus(int id)

    }
}
