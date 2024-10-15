using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Core.RequestDTO
{
    public class UpdateUserDTO
    {
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
    }
}
