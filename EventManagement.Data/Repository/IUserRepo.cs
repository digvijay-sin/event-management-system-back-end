using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public interface IUserRepo
    {
        Task<UserResponseDTO> CreateUser(RegistrationRequestDTO userDto);

        Task<bool> DeleteUser(int UserId);

        Task<UserResponseDTO> UpdateUser(int UserId, UpdateUserDTO userDto);

        Task<UserResponseDTO> GetUserById(int UserId);

        Task<List<UserResponseDTO>> GetUsers();

    }
}
