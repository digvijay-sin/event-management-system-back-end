﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId { get; set; }

        public int EventId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? Status { get; set; }

        public Event Event { get; set; }
    }
}
