using Models.DTOs.User.ForgotPassword;
using Models.DTOs.User.Login;
using Models.DTOs.User.Login.UserRefreshTokenDto;
using Models.Entities;
using System.Security.Claims;

namespace Core.Repositories.Specific
{
    public interface IJWTManagerRepository : IRepository<User>
    {
        Tokens GenerateJWTTokens(int id, string name, string roleName,bool rememberMe);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        UserAccessTokenDto GetTokenByRefreshToken(string refreshToken);
        void ResetPassword(ResetPasswordDto model);
        Task ForgotPassword(ForgotPasswordDto model, string origin);
        Task SendPasswordResetEmail(int userId, string email, string origin);
        ForgotTokenDto GenerateJWTTokenForResetPassword(int id);
        int GetUserIdFromToken(string token);
    }
}
