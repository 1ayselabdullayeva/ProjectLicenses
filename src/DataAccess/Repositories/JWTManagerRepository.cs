using Azure.Core;
using Core.Repositories;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
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
using System.Security.Principal;
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
                    Expires = DateTime.Now.AddMinutes(30),
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

                    Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(7),
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

        public void ResetPassword(ResetPasswordDto model)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.UTF8.GetBytes(model.Password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            var hashedPasswordString = Convert.ToBase64String(hashedPassword);

            // Kullanıcıyı e-posta adresine göre bul
            var account = _userRepository.GetSingle(x => x.Id == model.Id);

            if (account != null)
            {
                account.Password = hashedPasswordString;
                _userRepository.Edit(account);
                _userRepository.Save();
            }
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
            try
            {
                var account = _userRepository.GetSingle(x => x.Email == model.Email);

                await SendPasswordResetEmail(account.Id, account.Email, origin);
            }
            catch(Exception ex)
            {
                throw;
            }
            
            
        }
        //public async Task sendPasswordResetEmail(string email, string origin)
        //{
        //    var ResetToken = randomTokenString();
        //    string message;
        //    if (!string.IsNullOrEmpty(origin))
        //    {
        //        var resetUrl = $"{origin}/account/reset-password?token={ResetToken}";
        //        message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
        //                     <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
        //    }
        //    else
        //    {
        //        message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
        //                     <p><code>{ResetToken}</code></p>";
        //    }

        //    await _emailSenderServices.SendEmail(
        //        toEmail: email,
        //        subject: "Sign-up Verification API - Reset Password",
        //        message: $"{message}"
        //    );      
        //}
        public async Task SendPasswordResetEmail(int userId, string email,string origin)
        {
            string message;

            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password/{userId}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                     <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please navigate to the following URL to reset your password:</p>
                     <p>/account/reset-password/{userId}</p>";
            }

            await _emailSenderServices.SendEmail(email, "Sign-up Verification API - Reset Password", message);
            //    toEmail: email,
            //    subject: "Sign-up Verification API - Reset Password",
            //    message: $"{message}"
            //);
        }

        //void IJWTManagerRepository.sendPasswordResetEmail(string email, string origin)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
