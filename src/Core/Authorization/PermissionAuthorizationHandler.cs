using Microsoft.AspNetCore.Authorization;

namespace Core.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permission = context.User.Claims.Where(c => c.Type == "Permissions").Select(c => c.Value);

            if(permission.Contains(requirement.Permission ) )
                { 
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
