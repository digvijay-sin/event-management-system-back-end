using AutoMapper;
using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Context;
using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository.Implementations
{
    public class UserRepository : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserResponseDTO> CreateUser(RegistrationRequestDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            int count = await _context.SaveChangesAsync();
            return count > 0;
        }

        public async Task<UserResponseDTO> UpdateUser(int userId, UpdateUserDTO userDto)
        {
            var user = await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return null;
            }

            _mapper.Map(userDto, user);

            int count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                throw new Exception("Error updating user.");
            }

            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO?> GetUserById(int userId)
        {
            var user = await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserId == userId);
            return user != null ? _mapper.Map<UserResponseDTO>(user) : null;
        }

        public async Task<List<UserResponseDTO>> GetUsers()
        {
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return _mapper.Map<List<UserResponseDTO>>(users);
        }

    }
}
