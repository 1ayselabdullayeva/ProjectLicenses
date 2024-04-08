using Application.Helper.Hasher;
using Core.Exceptions;
using Core.Repositories.Specific;
using Core.Services;
using MimeKit.Encodings;
using Models.DTOs.Permissions.Get;
using Models.DTOs.User.Login;
using Models.DTOs.User.Register;
using Models.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
namespace Application.Services
{
    public class JWTService : IJWTServices
    {   private readonly IUserRepository _userRepository;
        private readonly IRolesServices _rolesServices;
        private readonly IJWTManagerRepository _jwtManagerRepository;
        private readonly IEmailSenderServices _emailSenderServices;
        private readonly IPermissionsServices _permissionsServices;

        public JWTService(IUserRepository userRepository, IRolesServices rolesServices, IJWTManagerRepository jwtManagerRepository, IEmailSenderServices emailSenderServices, IPermissionsServices permissionsServices)
        {
            _userRepository = userRepository;
            _rolesServices = rolesServices;
            _jwtManagerRepository = jwtManagerRepository;
            _emailSenderServices = emailSenderServices;
            _permissionsServices = permissionsServices;
        }
        public void AddUserRefreshTokens(User user)
        {
            var existingUser = _userRepository.GetSingle(x=>x.Id==user.Id); 
            if (existingUser != null)
            {
                existingUser.RefreshToken = user.RefreshToken;
                _userRepository.Edit(existingUser);
            }
            else
            {
                _userRepository.Add(user);
            }
            _userRepository.Save();
        }
        public void DeleteUserRefreshTokens(int Id, string refreshToken)
        {
            var item = _userRepository.GetSingle(x => x.Id == Id && x.RefreshToken == refreshToken);
            if (item != null)
            {
                _userRepository.Remove(item);
                _userRepository.Save();
            }
        }

        public UserLoginResponseDto Login(UserLoginDto usersdata)
        {
            var user = _userRepository.GetSingle(u => u.Email == usersdata.Email.ToLower());
            if (user == null)
                throw new ResourceNotFoundException("Istifadeci movcud deyil");
            var hashedPasswordString = PasswordHasherDto.Hasher(usersdata.Password);
            if (user.Password != hashedPasswordString)
                throw new ResourceNotFoundException("Istifadeci movcud deyil");

            var userRoles = _rolesServices.GetRoleName(user.RolesId);
            var userPermissions = _permissionsServices.GetPermissions(user.RolesId);

            var token = _jwtManagerRepository.GenerateJWTTokens(user.Id, user.FirstName, userRoles.RoleName, usersdata.RememberMe);
            var response = new UserLoginResponseDto
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CompanyName = user.CompanyName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                RolesId = user.RolesId,
                Roles = new Models.DTOs.Roles.RoleWithPermissions.RoleWithPermissionsWithDto
                {
                    RoleName = userRoles.RoleName,
                    Permissions = userPermissions.Select(p => new GetPermissionsResponseDto { PermissionName = p.PermissionName }).ToList()
                }
            }; 

            if (token == null)
                throw new BadRequestException("Failed to generate JWT token");

            User users = new User
            {
                Id = user.Id,
                RefreshToken = token.RefreshToken
            };
             AddUserRefreshTokens(users);

            return response;
        }

    public void LogOut(int userId)
        {
            var userTokens = _userRepository.GetSingle(u => u.Id == userId);
            userTokens.RefreshToken= null;
            _userRepository.Edit(userTokens);
            _userRepository.Save();
        }
        public async Task<UserRegisterResponseDto> Register(UserRegisterDto userRegister)
        
        {

            var user = _userRepository.GetSingle(x => x.Email == userRegister.Email, false);
            if(user != null)
            {
                throw new BadRequestException("Istifadeci artiq sistemde movcuddur");
            };
            var defaultRole = _rolesServices.GetDefaultRole();
                userRegister.RolesId = defaultRole.Id;
                var hashedPasswordString = PasswordHasherDto.Hasher(userRegister.Password);

                var userEntity = new User
                {
                    FirstName = userRegister.FirstName,
                    LastName = userRegister.LastName,
                    Email = userRegister.Email.ToLower(),
                    Password = hashedPasswordString,
                    CompanyName = userRegister.CompanyName,
                    RolesId = userRegister.RolesId,
                    PhoneNumber = userRegister.PhoneNumber
                };
                await _userRepository.AddAsync(userEntity);
                await _emailSenderServices.SendEmail(userEntity.Email, "User Registered", "Welcome to our web site");

                var userToReturn = new UserRegisterResponseDto
                {
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    Email = userEntity.Email,
                    CompanyName = userEntity.CompanyName,
                    RolesId = userEntity.RolesId,
                    PhoneNumber = userEntity.PhoneNumber
                };
                return userToReturn;
            
        }
    }
}

