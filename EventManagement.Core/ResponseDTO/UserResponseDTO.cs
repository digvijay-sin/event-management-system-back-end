using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Core.ResponseDTO
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string MobileNumber{ get; set; }
        public string Organisation { get; set; }
        public string Address{ get; set; }
        public RoleResponseDTO Role { get; set; }
    }
}
