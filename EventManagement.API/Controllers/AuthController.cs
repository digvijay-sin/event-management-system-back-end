using EventManagement.API.Service;
using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _authRepository;
        private readonly TokenService _tokenService;

        public AuthController(IAuthRepo authRepository, TokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO creds)
        {
            if (creds == null || string.IsNullOrEmpty(creds.Email) || string.IsNullOrEmpty(creds.Password))
            {
                return BadRequest("Invalid login request.");
            }
            try {
                var user = await _authRepository.AuthenticateUser(creds);


                if (user == null)
                {
                    return NotFound("User not found.");
                }

                string token = _tokenService.GetToken(user);

                AuthResponseDTO response = new()
                {
                    Token = token,
                    UserId = user.UserId,
                    Username = user.Username
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred while Authenticating the user.");
            }

        }
    }
}
