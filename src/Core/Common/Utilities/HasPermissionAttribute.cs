using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Core.Common.Utilities
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        private readonly string _permission;
        public HasPermissionAttribute(string permission) : base(policy: Convert.ToString(permission))
        {
            _permission = permission;
        }

    }
}
