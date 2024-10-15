using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Core.ResponseDTO
{
    public class EventWithRsvpsDTO
    {
        public int EventId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public int AttendeeCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<RsvpResponseDTO> Rsvps{ get; set; }
    }
}
