using Core.Repositories.Specific;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Licenses.GetById;
using Models.DTOs.Permissions.Get;
using Models.DTOs.Roles.GetRole;
using Models.DTOs.Roles.GetRoleName;
using Models.Entities;

namespace Business.Services
{
    public class RolesService : IRolesServices
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IPermissionsRepository _permissionsRepository;
        public RolesService(IRolesRepository rolesRepository, IPermissionsRepository permissionsRepository)
        {
            _rolesRepository = rolesRepository;
            _permissionsRepository = permissionsRepository;
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
            var response = role.Select(role => new GetRoleNameResponeDbo
            {
                Id = role.Id,
                RoleName = role.RoleName,
            }).ToList();
            return response;
        }

        public GetRoleNameResponeDbo GetRoleName(int id)
        {
            var role = _rolesRepository.GetSingle(role => role.Id == id);
            var response = new GetRoleNameResponeDbo
            {
                RoleName = role.RoleName
            };
            return response;
        }
        public List<GetPermissionsResponseDto> GetPermissions(int id)
        {
            var role = _permissionsRepository.GetAll(role => role.RolesId == id);
            var responseList = new List<GetPermissionsResponseDto>();
            foreach (var item in role)
            {
                var response = new GetPermissionsResponseDto
                {
                    //Id = item.Id,
                    PermissionName = item.PermissionName,
                };
                responseList.Add(response);
            }
           
            return responseList;
        }


    }
}
