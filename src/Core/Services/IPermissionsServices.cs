using Models.DTOs.Permissions.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IPermissionsServices
    {
        List<GetPermissionsResponseDto> GetPermissions(int id);
    }
}
