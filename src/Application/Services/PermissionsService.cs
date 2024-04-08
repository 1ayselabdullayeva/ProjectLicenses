using Core.Repositories.Specific;
using Core.Services;
using Models.DTOs.Permissions.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PermissionsService : IPermissionsServices
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public PermissionsService(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public List<GetPermissionsResponseDto> GetPermissions(int id)
        {

            var role = _permissionsRepository.GetAll(role => role.RolesId == id);
            var responseList = new List<GetPermissionsResponseDto>();
            foreach (var item in role)
            {
                var response = new GetPermissionsResponseDto
                {
                    PermissionName = item.PermissionName,
                };
                responseList.Add(response);
            }

            return responseList;
        }
    }
}
