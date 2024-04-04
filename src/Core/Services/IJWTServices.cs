using Models.DTOs.User.Login;
using Models.DTOs.User.Register;
using Models.Entities;

namespace Core.Services
{
    public interface IJWTServices
    {

        void AddUserRefreshTokens(User user);
        void DeleteUserRefreshTokens(int Id, string refreshToken);
        Task<UserRegisterResponseDto> Register(UserRegisterDto userRegister);
        Tokens Login(UserLoginDto login);
        void LogOut(int id);
    }

}
