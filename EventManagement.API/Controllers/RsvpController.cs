using EventManagement.API.Service;
using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Models;
using EventManagement.Data.Repository;
using EventManagement.Data.Repository.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagement.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RsvpController : ControllerBase
    {
        private readonly IRsvpRepo _rsvpRepository;
        private readonly EmailSender _emailSender;

        public RsvpController(IRsvpRepo rsvpRepository,  EmailSender email)
        {
            _rsvpRepository = rsvpRepository;
            _emailSender = email;
        }

        [AllowAnonymous]
        [HttpGet("respond")]
        [ProducesResponseType(typeof(RsvpResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RespondToRsvp(string status, int rsvpId)
        {
            // Model validation
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("RSVP status is required.");
            }

            try
            {
                var updatedRsvp = await _rsvpRepository.UpdateRsvpStatus(rsvpId, status);
                return Ok(updatedRsvp);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("allAttendees/{eventId}")]
        [ProducesResponseType(typeof(List<RsvpResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RsvpResponseDTO>>> GetRsvpsByEvent(int eventId)
        {
            try
            {
                var rsvps = await _rsvpRepository.GetRsvpsByEventId(eventId);
                if (rsvps == null || rsvps.Count == 0)
                {
                    return NotFound("No RSVPs found for this event.");
                }
                return Ok(rsvps);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("sendMail/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendMails(int eventId)
        {
            try
            {
                var rsvps = await _rsvpRepository.GetRsvpsByEventId(eventId);
                if (rsvps == null || rsvps.Count == 0)
                {
                    return NotFound("No RSVPs found for this event.");
                }

                await _emailSender.SendInvitationEmailAsync(rsvps);
                return Ok("Invitation emails sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRsvp(int id)
        {
            try
            {
                var result = await _rsvpRepository.DeleteRsvp(id);
                if (!result)
                {
                    return NotFound("RSVP not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(RsvpResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RsvpResponseDTO>> UpdateRsvp(int id, [FromBody] UpdateRsvpRequestDTO rsvpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedRsvp = await _rsvpRepository.UpdateRsvp(id, rsvpDto);
                if (updatedRsvp == null)
                {
                    return NotFound("RSVP not found.");
                }

                return Ok(updatedRsvp);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        private int GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null) {
                return 0;
            }
            return int.Parse(userId);
        }
    }
}
