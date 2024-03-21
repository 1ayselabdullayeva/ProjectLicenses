using Azure.Core;
using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.User.Login.UserRefreshTokenDto;
using Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Tokens = Models.DTOs.User.Login.Tokens;

namespace DataAccess.Repositories
{
    public class JWTManagerRepository :Repository<User>, IJWTManagerRepository
    {
        private readonly IConfiguration _configuration;

        public JWTManagerRepository(DbContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            _configuration = configuration;
        }

        public Tokens GenerateJWTTokens(int id, string userName, string roleName, bool rememberMe)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]);
                var refreshkey = Encoding.UTF8.GetBytes(_configuration["Jwt1:RefreshKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                   new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                   new Claim(ClaimTypes.Name, userName),
                   new Claim(ClaimTypes.Role,roleName)
                  }),
                    Expires = DateTime.Now.AddSeconds(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var refreshTokenDesc = new SecurityTokenDescriptor
                {

                    Subject = new ClaimsIdentity(new Claim[]
                  {
                      new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                      new Claim(ClaimTypes.Name, userName),
                      new Claim(ClaimTypes.Role, roleName)
                  }),

                    Expires = rememberMe ? DateTime.Now.AddSeconds(30) : DateTime.Now.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(refreshkey), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = tokenHandler.CreateToken(refreshTokenDesc);
                return new Tokens
                {
                    AccessToken = tokenHandler.WriteToken(token),
                    RefreshToken = tokenHandler.WriteToken(refreshToken)
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UserAccessTokenDto GetTokenByRefreshToken(string refreshToken)
        {
               var principal = GetPrincipalFromExpiredToken(refreshToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value; 
                var roleName = principal.FindFirst(ClaimTypes.Role).Value;
              var username= principal.FindFirst(ClaimTypes.Name).Value;
            var newAccessToken = GenerateJWTTokens(int.Parse(userId), username, roleName,false);

                var response = new UserAccessTokenDto
                {
                    AccesToken = newAccessToken.AccessToken
                };
                return response;
        }
        public bool IsValid(string token)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
                return jwtSecurityToken.ValidTo > DateTime.UtcNow;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {

            var Key = Encoding.UTF8.GetBytes(_configuration["JWT1:RefreshKey"]);
            IdentityModelEventSource.ShowPII = true;
            IdentityModelEventSource.LogCompleteSecurityArtifact = true;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            else
            {
                return null;
            }



        }
    }
}
