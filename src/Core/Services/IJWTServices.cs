using Models.DTOs.User.Login;
using Models.DTOs.User.Login.AddRefreshToken;
using Models.DTOs.User.Register;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
