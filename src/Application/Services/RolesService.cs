using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Repositories;
using Models.DTOs.Roles.GetRole;
using Models.DTOs.Roles.GetRoleName;
using Models.Entities;

namespace Business.Services
{
    public class RolesService : IRolesServices
    {
        private readonly IRolesRepository _rolesRepository;
        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }
        public RolesGetRoleDbo GetDefaultRole()
        {
            var defaultvalue = _rolesRepository.GetSingle(role => role.Id == 2);
            var response = new RolesGetRoleDbo
            {
                Id = defaultvalue.Id,
            };
            return response;
        }

        public GetRoleNameResponeDbo GetRoleName(int id)
        {
           var role=_rolesRepository.GetSingle(role => role.Id == id);
            var response = new GetRoleNameResponeDbo
            {
                RoleName = role.RoleName
            };
            return response;
        }
    }
}
