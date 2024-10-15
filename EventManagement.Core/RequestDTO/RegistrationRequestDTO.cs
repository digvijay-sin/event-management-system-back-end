using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Core.RequestDTO
{
    public class RegistrationRequestDTO
    {
        public string Username { get; set; }

        public string MobileNumber { get; set; }

        public string Organisation { get; set; }

        public string Email { get; set; }

        public string  Address { get; set; }

        public string Password { get; set; }
    }
}
