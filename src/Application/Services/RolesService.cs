using Core.Repositories.Specific;
using Core.Services;
using Models.DTOs.Roles.GetRole;
using Models.DTOs.Roles.GetRoleName;

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
            var defaultvalue = _rolesRepository.GetSingle(role => role.RoleName == "Customer").Id;
            var response = new RolesGetRoleDbo
            {
                Id = defaultvalue,
            };
            return response;
        }

        public List<GetRoleNameResponeDbo> GeTRole()
        {
           var role = _rolesRepository.GetAll();
           var response=role.Select(role =>new GetRoleNameResponeDbo
           {
               Id=role.Id,
               RoleName=role.RoleName,
           }).ToList();
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
