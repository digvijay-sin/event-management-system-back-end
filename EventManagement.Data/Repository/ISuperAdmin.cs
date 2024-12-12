using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public interface ISuperAdmin
    {
        public Task<bool> CreateRole(RoleRequestDTO role);
        public Task<UserResponseDTO> AssignRole(AddUserRoleDTO addUserRole);
    }
}
