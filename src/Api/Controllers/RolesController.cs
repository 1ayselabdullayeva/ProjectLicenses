using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize("Admin")]
    [ApiController]
    public class RolesController : ControllerBase
    {
       private readonly IRolesServices _rolesServices;

        public RolesController(IRolesServices rolesServices)
        {
            _rolesServices = rolesServices;
        }

        [HttpGet("getRoles")]
        [Authorize("Admin")]
        public IActionResult GetAllTickets()
        {
            var response = _rolesServices.GeTRole();
            return Ok(response);
        }
    }
}
