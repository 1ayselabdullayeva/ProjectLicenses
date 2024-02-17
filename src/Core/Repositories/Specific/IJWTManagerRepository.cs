using Models.DTOs.User.Login;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Specific
{
    public interface IJWTManagerRepository: IRepository<UserRefreshToken>
    {
         Tokens GenerateToken(int id,string name, string roleName);
         Tokens GenerateRefreshToken(int id,string name,  string roleName);
         ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
