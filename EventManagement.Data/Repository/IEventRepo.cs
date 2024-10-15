using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public interface IEventRepo
    {


        public Task<List<EventResponseDTO>> GetAllEvents(int UserId);

        public Task<EventResponseDTO> CreateEvent(CreateEventRequestDTO eventDto, int UserId);

        public Task<EventResponseDTO> UpdateEvent( UpdateEventRequestDTO eventDto, int UserId);
        
        public Task<bool> DeleteEvent(int eventId, int UserId);

        public Task<EventResponseDTO> GetEventById(int eventId, int UserId);

        public Task<List<EventWithRsvpsDTO>> GetEventsWithRsvpsWithPositiveStatus(DateTime date);

        public Task SetFinishedEvents(CancellationToken stoppingToken);
        public Task SetOngoingEvents(CancellationToken stoppingToken);
        public Task SetUpcomingEvents(CancellationToken stoppingToken);

    }
}
