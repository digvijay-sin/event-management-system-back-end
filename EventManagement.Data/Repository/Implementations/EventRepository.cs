using AutoMapper;
using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Context;
using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository.Implementations
{
    public class EventRepository : IEventRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public EventRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventResponseDTO> CreateEvent(CreateEventRequestDTO eventDto, int userId)
        {
            var @event = _mapper.Map<Event>(eventDto);
            @event.UserId = userId;
            @event.CreatedAt = DateTime.UtcNow;
            @event.UpdatedAt = DateTime.UtcNow;
            @event.Status = "Upcoming";

            await _context.AddAsync(@event);
            int count = await _context.SaveChangesAsync();

            if (count == 0)
            {
                throw new Exception("Failed to create the event. Please try again."); 
            }

            return _mapper.Map<EventResponseDTO>(@event);
        }

        public async Task<bool> DeleteEvent(int eventId, int userId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            if (@event == null)
            {
                throw new KeyNotFoundException("Event not found."); 
            }

            if (@event.UserId != userId)
            {
                throw new UnauthorizedAccessException("User not authorized to delete this event."); 
            }

            _context.Events.Remove(@event);
            int count = await _context.SaveChangesAsync();

            return count > 0; 
        }

        public async Task<List<EventResponseDTO>> GetAllEvents(int userId)
        {
            var events = await _context.Events.Where(e => e.UserId == userId).Include(e => e.Rsvps).ToListAsync();
            return _mapper.Map<List<EventResponseDTO>>(events);
        }

        public async Task<EventResponseDTO> GetEventById(int eventId, int userId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            if (@event == null || @event.UserId != userId)
            {
                throw new KeyNotFoundException("Event not found or user not authorized.");
            }
            return _mapper.Map<EventResponseDTO>(@event);
        }

        public async Task<EventResponseDTO> UpdateEvent(UpdateEventRequestDTO eventDto, int userId)
        {
            var @event = await _context.Events.FindAsync(eventDto.EventId);
            if (@event == null)
            {
                throw new KeyNotFoundException("Event not found."); 
            }

            if (@event.UserId != userId)
            {
                throw new UnauthorizedAccessException("User not authorized to update this event.");
            }

            @event.UpdatedAt = DateTime.UtcNow;
            _mapper.Map(eventDto, @event); 
            _context.Entry(@event).State = EntityState.Modified;

            int count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                throw new Exception("Failed to update the event. Please try again."); 
            }

            return _mapper.Map<EventResponseDTO>(@event);
        }

        public async Task<List<EventWithRsvpsDTO>> GetEventsWithRsvpsWithPositiveStatus(DateTime date)
        {
            var events = await _context.Events
                .Where(e => e.EventDate == date.Date)
                .Include(e => e.Rsvps.Where(r => r.Status == "going" || r.Status == "maybe"))
                .ToListAsync();
            var eventsWithRsvps = _mapper.Map<List<EventWithRsvpsDTO>>(events);

            if (eventsWithRsvps.Count == 0)
            {

            }
            return eventsWithRsvps;
        }

        public async Task SetFinishedEvents(CancellationToken stoppingToken)
        {
            var now = DateTime.Now;

            var finishedEvents = await _context.Events
                .Where(e => e.EventDate.Add(new TimeSpan(e.DurationDays, e.DurationHours, e.DurationMinutes)) < now && e.Status != "Finished")
                .ToListAsync(stoppingToken);

            foreach (var eventItem in finishedEvents)
            {
                eventItem.Status = "Finished";
            }

            await _context.SaveChangesAsync(stoppingToken);
        }


        public async Task SetOngoingEvents(CancellationToken stoppingToken)
        {
            var now = DateTime.Now;
            new TimeSpan();
            var ongoingEvents = await _context.Events
              .Where(e => e.EventDate <= now && e.EventDate.Add(new TimeSpan(e.DurationDays, e.DurationHours, e.DurationMinutes)) > now && e.Status != "Ongoing")
              .ToListAsync(stoppingToken);

            foreach (var eventItem in ongoingEvents)
            {
                eventItem.Status = "Ongoing";
            }

            await _context.SaveChangesAsync(stoppingToken);
        }

        public async Task SetUpcomingEvents(CancellationToken stoppingToken)
        {
            var now = DateTime.Now;

            var upcomingEvents = await _context.Events
               .Where(e => e.EventDate > now && e.Status != "Upcoming")
               .ToListAsync(stoppingToken);

            foreach (var eventItem in upcomingEvents)
            {
                eventItem.Status = "Upcoming";
            }

            await _context.SaveChangesAsync(stoppingToken);
        }
    }
}
