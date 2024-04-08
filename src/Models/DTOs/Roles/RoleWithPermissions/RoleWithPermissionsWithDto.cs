using Models.DTOs.Permissions.Get;

namespace Models.DTOs.Roles.RoleWithPermissions
{
    public class RoleWithPermissionsWithDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<GetPermissionsResponseDto> Permissions { get; set; }
    }
}
