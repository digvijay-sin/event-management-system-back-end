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
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepository;

        public UserController(IUserRepo userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationRequestDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data cannot be null.");
            }

            try
            {
                var userResponse = await _userRepository.CreateUser(userDto);
                return CreatedAtAction(nameof(GetUser), new { userId = userResponse.UserId }, userResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error has occurred while creating the user: {ex.Message}");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("delete/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _userRepository.DeleteUser(userId);
                if (!result)
                {
                    return NotFound("User not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error has occurred while deleting the user: {ex.Message}");
            }
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPut("update/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data cannot be null.");
            }

            try
            {
                var userResponse = await _userRepository.UpdateUser(userId, userDto);
                if (userResponse == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error has occurred while updating the user: {ex.Message}");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("getUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var userResponse = await _userRepository.GetUserById(userId);
                if (userResponse == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error has occurred while retrieving the user: {ex.Message}");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error has occurred while retrieving users: {ex.Message}");
            }
        }

    }
}
