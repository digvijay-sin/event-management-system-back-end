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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRole([FromBody] RoleRequestDTO role)
        {
            if (role == null)
            {
                return BadRequest("Role data is required.");
            }

            try
            {
                bool status = await _superadminRepository.CreateRole(role);
                if (!status)
                {
                    return StatusCode(500, "An unexpected error has occurred while creating the role.");
                }

                return CreatedAtAction(nameof(AddRole), new { role }, "Successfully! Role Created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error has occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("assignRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AddUserRoleDTO addUserRole)
        {
            if (addUserRole == null)
            {
                return BadRequest("User role data is required.");
            }

            try
            {
                var user = await _superadminRepository.AssignRole(addUserRole);
                if (user == null)
                {
                    return NotFound("User or Role not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error has occurred: {ex.Message}");
            }
        }

    }
}
