using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Core.ResponseDTO
{
    public class RsvpResponseDTO
    {
        public int  RsvpId { get; set; }

        public int EventId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }

        public DateTime ResponseDate { get; set; }

        public EventResponseDTO Event{ get; set; }
    }   
}
