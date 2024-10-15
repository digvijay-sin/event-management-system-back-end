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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEvent(CreateEventRequestDTO eventDto)
        {
            if (eventDto.CsvFile == null || eventDto.CsvFile.Length == 0 || !eventDto.CsvFile.FileName.EndsWith(".csv"))
            {
                return BadRequest("A valid CSV file is required.");
            }

            var createdEvent = await _eventRepository.CreateEvent(eventDto, GetUserId());
            
            var rsvps = await CSVHelper.ProcessCSVFile(eventDto.CsvFile, createdEvent);            

            UpdateEventRequestDTO AttendeeCountUpdate = new() {
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
            
            //await _emailSender.send(rsvps, createdEvent.Title);

            //return CreatedAtAction(nameof(GetEvent), new {id = createdEvent.EventId}, createdEvent);
            return Ok(createdEvent);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost("delete/{eventId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var status  = await _eventRepository.DeleteEvent(eventId, GetUserId());
            if (!status)
            {
                return Unauthorized();
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("update/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEvent(int eventId, UpdateEventRequestDTO eventDto)
        {
            if(eventId != eventDto.EventId)
            {
                return BadRequest();
            }

            var eventResDto = await _eventRepository.UpdateEvent(eventDto, GetUserId());
            if (eventResDto == null)
            {

            }           
            return Ok(eventResDto);
        }

        [Authorize(Roles = "Admin")]

        [HttpGet("GetEvent/{eventId}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetEvent(int eventId)
        {
            var eventResDto = await _eventRepository.GetEventById(eventId, GetUserId());
            if (eventResDto == null)
            {
                return Unauthorized();
            }
            return Ok(eventResDto);
        }

        [Authorize]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventRepository.GetAllEvents(GetUserId());
            if (events.Count == 0)
            {

            }
            
            
            return Ok(events);
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
