using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User.Login;
using Models.DTOs.User.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IAuthSevices
    {
        Task<Tokens> Login(UserLoginDto usersdata);
        Task<UserRegisterResponseDto> Register(UserRegisterDto userRegister);
        void LogOut(int id);
    }
}
