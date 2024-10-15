using EventManagement.API.Service;
using EventManagement.Core.RequestDTO;
using EventManagement.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdmin _superadminRepository;
        private readonly TokenService _tokenService;

        public SuperAdminController(ISuperAdmin superadminRepository, TokenService tokenService)
        {
            _superadminRepository = superadminRepository;
            _tokenService = tokenService;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("addRole")]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> AddRole([FromBody] RoleRequestDTO role)
        {
            if (role == null)
            {
                return BadRequest();
            }
            bool status = await _superadminRepository.CreateRole(role);
            if (!status)
            {
                return StatusCode(500, "An Unexpected Error has Occurred!");
            }
            return StatusCode(201, "Successfully! Role Created");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("assignRole")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AddUserRoleDTO addUserRole)
        {
            if (addUserRole == null)
            {
                return BadRequest();
            }
            var user = await _superadminRepository.AssignRole(addUserRole);

            if (user == null)
            {
                return StatusCode(500, "An Unexpected Error has Occurred!");
            }
            return Ok(user);
        }
    }
}
