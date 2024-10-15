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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(RegistrationRequestDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data cannot be null.");
            }

            var userResponse = await _userRepository.CreateUser(userDto);
            return CreatedAtAction(nameof(GetUser), new { userId = userResponse.UserId }, userResponse);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("delete/{userId}")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userRepository.DeleteUser(userId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPut("update/{userId}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data cannot be null.");
            }

            var userResponse = await _userRepository.UpdateUser(userId, userDto);
            if (userResponse == null)
            {
                return NotFound();
            }

            return Ok(userResponse);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("getUser/{userId}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> GetUser(int userId)
        {
            var userResponse = await _userRepository.GetUserById(userId);
            if (userResponse == null)
            {
                return NotFound();
            }

            return Ok(userResponse);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("getAll")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
 
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            if(users == null)
            {

            }
            return Ok(users);
        }
    }
}
