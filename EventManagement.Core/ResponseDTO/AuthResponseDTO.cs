using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Core.ResponseDTO
{
    public class AuthResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string? ProfileImage { get; set; }
    }
}
