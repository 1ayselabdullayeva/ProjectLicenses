using Models.DTOs.Roles.GetRole;
using Models.DTOs.Roles.GetRoleName;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.GetById;

namespace Core.Services
{
	public interface IRolesServices
	{
		public RolesGetRoleDbo GetDefaultRole();
		public GetRoleNameResponeDbo GetRoleName(int id);
	}
}
