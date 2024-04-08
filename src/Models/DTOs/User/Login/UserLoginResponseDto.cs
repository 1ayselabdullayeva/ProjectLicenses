using Models.DTOs.Roles.RoleWithPermissions;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.User.Login
{
    public class UserLoginResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int RolesId { get; set; }
        public RoleWithPermissionsWithDto Roles { get; set; }
        
       
    }
}
