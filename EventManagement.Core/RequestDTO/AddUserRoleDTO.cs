using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Core.RequestDTO
{
    public class AddUserRoleDTO
    {
        public int UserId{ get; set; }

        public int RoleId { get; set; }
    }
}
