using Models.DTOs.Roles.GetRole;
using Models.DTOs.Roles.GetRoleName;

namespace Core.Services
{
	public interface IRolesServices
	{
		 RolesGetRoleDbo GetDefaultRole();
		 GetRoleNameResponeDbo GetRoleName(int id);
		 List<GetRoleNameResponeDbo> GeTRole();
	}
}
