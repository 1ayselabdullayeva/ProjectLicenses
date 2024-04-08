using Models.DTOs.Permissions.Get;
using Models.DTOs.Roles.GetRole;
using Models.DTOs.Roles.GetRoleName;
using Models.Entities;

namespace Core.Services
{
	public interface IRolesServices
	{
		 RolesGetRoleDbo GetDefaultRole();
		 GetRoleNameResponeDbo GetRoleName(int id);
		 List<GetRoleNameResponeDbo> GeTRole();
        List<GetPermissionsResponseDto> GetPermissions(int id);

    }
}
