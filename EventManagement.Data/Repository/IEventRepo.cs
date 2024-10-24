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


        Task<EventResponseDTO> CreateEvent(CreateEventRequestDTO eventDto, int userId);
        Task<bool> DeleteEvent(int eventId, int userId);
        Task<List<EventResponseDTO>> GetAllEvents(int userId);
        Task<EventResponseDTO> GetEventById(int eventId, int userId);
        Task<EventResponseDTO> UpdateEvent(UpdateEventRequestDTO eventDto, int userId);

        public Task<List<EventWithRsvpsDTO>> GetEventsWithRsvpsWithPositiveStatus(DateTime date);

        public Task SetFinishedEvents(CancellationToken stoppingToken);
        public Task SetOngoingEvents(CancellationToken stoppingToken);
        public Task SetUpcomingEvents(CancellationToken stoppingToken);

    }
}
