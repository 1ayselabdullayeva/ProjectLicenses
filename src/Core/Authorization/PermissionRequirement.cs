using Microsoft.AspNetCore.Authorization;
using Models.Entities;

namespace Core.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission=permission;
        }
        public string Permission { get; }
    }

}
