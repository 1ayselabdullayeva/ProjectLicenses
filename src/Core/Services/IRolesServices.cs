using Models.DTOs.Roles.GetRole;
using Models.DTOs.Roles.GetRoleName;

namespace Core.Services
{
	public interface IRolesServices
	{
		public RolesGetRoleDbo GetDefaultRole();
		public GetRoleNameResponeDbo GetRoleName(int id);
	}
}
