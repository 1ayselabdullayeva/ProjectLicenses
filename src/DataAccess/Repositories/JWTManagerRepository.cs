using Core.Exceptions;
using Core.Repositories;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Permissions.Get;
using Models.DTOs.User.ForgotPassword;
using Models.DTOs.User.Login.UserRefreshTokenDto;
using Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tokens = Models.DTOs.User.Login.Tokens;

namespace DataAccess.Repositories
{
    public class JWTManagerRepository :Repository<User>, IJWTManagerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderServices _emailSenderServices;
        private readonly IUserRepository _userRepository;
        public JWTManagerRepository(DbContext dbContext, IConfiguration configuration, IEmailSenderServices emailSenderServices, IUserRepository userRepository) : base(dbContext)
        {
            _configuration = configuration;
            _emailSenderServices = emailSenderServices;
            _userRepository = userRepository;
        }

        public Tokens GenerateJWTTokens(int id, string userName, string roleName, List<GetPermissionsResponseDto> permissions, bool rememberMe)
        {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]);
                var refreshKey = Encoding.UTF8.GetBytes(_configuration["Jwt1:RefreshKey"]);

                var claims = new List<Claim>
                  {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, roleName)
                   };

                foreach (var permission in permissions)
                {
                    claims.Add(new Claim("Permissions", permission.PermissionName));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(30), 
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var refreshTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(7), 
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(refreshKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var accessToken = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

                return new Tokens
                {
                    AccessToken = tokenHandler.WriteToken(accessToken),
                    RefreshToken = tokenHandler.WriteToken(refreshToken)
                };
        }



        public UserAccessTokenDto GetTokenByRefreshToken(string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(refreshToken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            var roleName = principal.FindFirst(ClaimTypes.Role).Value;
            var username = principal.FindFirst(ClaimTypes.Name).Value;
            var permissions = principal.Claims.Where(c => c.Type == "Permissions").Select(c => new GetPermissionsResponseDto { PermissionName = c.Value }).ToList(); 
            var newAccessToken = GenerateJWTTokens(int.Parse(userId), username, roleName, permissions, false);

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
                throw new BadRequestException("Invalid token");
            }
            else
            {
                return principal;
            }
        }


        public ForgotTokenDto GenerateJWTTokenForResetPassword(int id)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt2:Forgot"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                   new Claim(ClaimTypes.NameIdentifier, id.ToString()),

                  }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var response = new ForgotTokenDto
                {
                    Token = tokenHandler.WriteToken(token)
                };
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void ResetPassword(ResetPasswordDto model)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.UTF8.GetBytes(model.Password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var hashedPasswordString = Convert.ToBase64String(hashedPassword);
            var userid=GetUserIdFromToken(model.Token);


            var account = _userRepository.GetSingle(x => x.Id == userid);

            if (account != null)
            {
                account.Password = hashedPasswordString;
                _userRepository.Edit(account);
                _userRepository.Save();
            }

        }
      
        public async Task ForgotPassword(ForgotPasswordDto model, string origin)
        {
                var account = _userRepository.GetSingle(x => x.Email == model.Email);
                if (account != null)
                {
                    await SendPasswordResetEmail(account.Id, account.Email, origin);
                }
                            
        }
        
        public async Task SendPasswordResetEmail(int userId, string email,string origin)
        {
            string message;
            var responseToken = GenerateJWTTokenForResetPassword(userId).Token;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password/{responseToken}";
                message = resetUrl;
            }

            else
            {
                message = $@"<p>Please navigate to the following URL to reset your password:</p>
                     <p>/account/reset-password/{responseToken}</p>";
            }

            await _emailSenderServices.SendEmail(email, "Sign-up Verification API - Reset Password", message);
            
        }

        public int GetUserIdFromToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_configuration["JWT2:Forgot"]);
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
            var userid = int.Parse(jwtSecurityToken.Claims.First(x=>x.Type=="nameid").Value);
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BadRequestException("Invalid token");
            }
            return userid;
        }

    }
}
