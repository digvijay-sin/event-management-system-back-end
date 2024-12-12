using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Models;
using EventManagement.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using EventManagement.API.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EventManagement.API.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepo _eventRepository;
        private readonly IRsvpRepo _rsvpRepository;        

        public EventController(IEventRepo eventRepository, IRsvpRepo rsvpRepository)
        {
            _eventRepository = eventRepository;
            _rsvpRepository = rsvpRepository;            
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(EventResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEvent(CreateEventRequestDTO eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (eventDto.CsvFile == null || eventDto.CsvFile.Length == 0 || !eventDto.CsvFile.FileName.EndsWith(".csv"))
            {
                return BadRequest("A valid CSV file is required.");
            }
            try
            {
                var createdEvent = await _eventRepository.CreateEvent(eventDto, GetUserId());
                var rsvps = await CSVHelper.ProcessCSVFile(eventDto.CsvFile, createdEvent);

                UpdateEventRequestDTO AttendeeCountUpdate = new()
                {
                    EventId = createdEvent.EventId,
                    Capacity = createdEvent.Capacity,
                    Description = createdEvent.Description,
                    EventDate = createdEvent.EventDate,
                    Location = createdEvent.Location,
                    Title = createdEvent.Title,
                    AttendeeCount = rsvps.Count
                };

                createdEvent = await _eventRepository.UpdateEvent(AttendeeCountUpdate, GetUserId());
                await _rsvpRepository.CreateRsvps(rsvps);

                return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.EventId }, createdEvent);

            }
            catch (Exception ex)
            {
                // Log the exception details (consider using a logging framework)
                // _logger.LogError(ex, "An error occurred while creating the event.");

                return StatusCode(500, "An unexpected error has occurred while creating the event.");
            }


        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete/{eventId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            if (eventId <= 0)
            {
                return BadRequest("Invalid event ID.");
            }
            try
            {
                var status = await _eventRepository.DeleteEvent(eventId, GetUserId());
                if (!status)
                {
                    return NotFound("Event not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details
                // _logger.LogError(ex, "An error occurred while deleting the event.");

                return StatusCode(500, "An unexpected error has occurred while deleting the event.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{eventId}")]
        [ProducesResponseType(typeof(EventResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEvent(int eventId, UpdateEventRequestDTO eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (eventId != eventDto.EventId)
            {
                return BadRequest("Event ID mismatch.");
            }
            try
            {                
                var eventResDto = await _eventRepository.UpdateEvent(eventDto, GetUserId());
                if (eventResDto == null)
                {
                    return NotFound("Event not found.");
                }

                return Ok(eventResDto);
            }
            catch (Exception ex)
            {
                // Log the exception details
                // _logger.LogError(ex, "An error occurred while updating the event.");

                return StatusCode(500, "An unexpected error has occurred while updating the event.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetEvent/{eventId}")]
        [ProducesResponseType(typeof(EventResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEvent(int eventId)
        {
            // Validate the eventId
            if (eventId <= 0)
            {
                return BadRequest("Invalid event ID.");
            }

            try
            {
                var eventResDto = await _eventRepository.GetEventById(eventId, GetUserId());
                if (eventResDto == null)
                {
                    return NotFound("Event not found.");
                }

                return Ok(eventResDto);
            }
            catch (Exception ex)
            {
                // Log the exception details
                // _logger.LogError(ex, "An error occurred while retrieving the event.");

                return StatusCode(500, "An unexpected error has occurred while retrieving the event.");
            }
        }


        [Authorize]
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<EventResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var events = await _eventRepository.GetAllEvents(GetUserId());
                
                if (events == null || !events.Any())
                {
                    return NotFound("No events found.");
                }

                return Ok(events);
            }
            catch (Exception ex)
            {
                // Log the exception details
                // _logger.LogError(ex, "An error occurred while retrieving the events.");

                return StatusCode(500, "An unexpected error has occurred while retrieving the events.");
            }
        }

        private int GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return 0;
            }

            return int.Parse(userId);
        }        
    }
}
