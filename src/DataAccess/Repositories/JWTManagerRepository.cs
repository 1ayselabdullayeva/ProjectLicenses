using Azure.Core;
using Core.Repositories;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
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
                    Expires = DateTime.Now.AddDays(30),
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

                    Expires = rememberMe ? DateTime.Now.AddSeconds(30) : DateTime.Now.AddSeconds(7),
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
            //var hashedPasswordString1 = HelperPasswordHasherDto.Hasher(model.Password);

            //// Kullanıcıyı e-posta adresine göre bul
            //var account = _userRepository.GetSingle(x => x.Id == model.Id);

            //if (account != null)
            //{
            //    account.Password = hashedPasswordString;
            //    _userRepository.Edit(account);
            //    _userRepository.Save();
            //}
        }
        //public string randomTokenString()
        //{
        //    using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        //    var randomBytes = new byte[40];
        //    rngCryptoServiceProvider.GetBytes(randomBytes);
        //    return BitConverter.ToString(randomBytes).Replace("-", "");
        //}
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
                //message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                //     <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
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
            //var userId = int.Parse(principal.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var userid = int.Parse(jwtSecurityToken.Claims.First(x=>x.Type=="nameid").Value);
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return userid;
        }
    }
}
