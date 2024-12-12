using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public interface IAuthRepo
    {
        public Task<UserResponseDTO> AuthenticateUser(LoginRequestDTO creds);
        
    }
}
