using Models.DTOs.User.Create;
using Models.DTOs.User.GetById;
using Models.DTOs.User.GetLicenses;
using Models.DTOs.User.Update;

namespace Core.Services
{
    public interface IUserServices
	{
		 Task<UserCreateResponseDto> Create(UserCreateDto request);
         UserGetByIdResponseDto GetById(int id);
         UserGetLicensesResponseDto GetLicensesStatus(int id);
         UserUpdateResponseDto UserEdit(int id,UserUpdateDto request);

    }
}
