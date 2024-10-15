using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EventManagement.Core.RequestDTO
{
    public class CreateEventRequestDTO
    {
        public string Title { get; set; }
        public string  Description { get; set; }

        
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public IFormFile CsvFile { get; set; }
        public int DurationDays { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
    }
}
