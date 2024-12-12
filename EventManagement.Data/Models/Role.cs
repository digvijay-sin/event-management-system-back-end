using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }     
        public string Description { get; set; }
        public List<User> Users{ get; set; }
    }
}
