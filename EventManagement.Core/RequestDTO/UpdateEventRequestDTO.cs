using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Core.RequestDTO
{
    public class UpdateEventRequestDTO
    {
        public int EventId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public int? AttendeeCount { get; set; }

        public TimeSpan Duration { get; set; }

    }
}
