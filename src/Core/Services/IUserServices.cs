using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.GetById;
using Models.DTOs.User.Create;

namespace Core.Services
{
	public interface IUserServices
	{
		public Task<UserCreateResponseDto> Create(UserCreateDto request);
	}
}
