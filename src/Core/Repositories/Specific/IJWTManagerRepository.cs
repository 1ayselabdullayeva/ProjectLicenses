using Models.DTOs.User.Login;
using Models.DTOs.User.Login.UserRefreshTokenDto;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Specific
{
    public interface IJWTManagerRepository : IRepository<User>
    {
        Tokens GenerateJWTTokens(int id, string name, string roleName,bool rememberMe);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        UserAccessTokenDto GetTokenByRefreshToken(string refreshToken);
    }
}
