using Models.DTOs.User.Login;
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
        
        Task<bool> IsValidUserAsync(UserLoginDto users);
        UserRefreshToken AddUserRefreshTokens(UserRefreshToken user);

        UserRefreshToken GetSavedRefreshTokens(int Id, string refreshtoken);
        void DeleteUserRefreshTokens(int Id, string refreshToken);
        Task<UserRegisterResponseDto> Register(UserRegisterDto userRegister);
    }

}
