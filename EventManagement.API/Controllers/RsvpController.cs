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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RespondToRsvp(string status, int rsvpId)
        {
            RsvpResponseDTO responseDto = new RsvpResponseDTO()
            {
                RsvpId = rsvpId,
                Status = status
            };

            if (string.IsNullOrEmpty(responseDto.Status))
            {
                return BadRequest("RSVP status is required.");
            }
            var updatedRsvp = await _rsvpRepository.UpdateRsvpStatus(responseDto.RsvpId, responseDto.Status);
            return Ok(updatedRsvp);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("allAttendees/{eventId}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<RsvpResponseDTO>>> GetRsvpsByEvent(int eventId)
        {
            var rsvps = await _rsvpRepository.GetRsvpsByEventId(eventId);            
            return Ok(rsvps);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("sendMail/{eventId}")]
        public async Task<IActionResult> SendMails(int eventId)
        {
            var rsvps = await _rsvpRepository.GetRsvpsByEventId(eventId);

            await _emailSender.SendInvitationEmailAsync(rsvps);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRsvp(int id)
        {
            var result = await _rsvpRepository.DeleteRsvp(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult<RsvpResponseDTO>> UpdateRsvp(int id, [FromBody] UpdateRsvpRequestDTO rsvpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRsvp = await _rsvpRepository.UpdateRsvp(id, rsvpDto);
            if (updatedRsvp == null)
            {
                return NotFound(); 
            }

            return Ok(updatedRsvp);
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
