using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public int? RoleId { get; set; }

        public Role Role { get; set; }

        public string Username { get; set; }

        public string MobileNumber { get; set; }

        public string Organisation { get; set; }

        public string Address { get; set; }

        public string  Email { get; set; }

        public string  Password { get; set; }

        public bool Exists { get; set; } = true;
        
        public List<Event> Events { get; set; }
    }
}
