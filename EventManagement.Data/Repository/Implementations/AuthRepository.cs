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
    public class AuthRepository : IAuthRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserResponseDTO?> AuthenticateUser(LoginRequestDTO creds)
        {
            var user = await _context.Users.Where(u => u.Email == creds.Email && u.Password == creds.Password).Include(u => u.Role).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserResponseDTO>(user);
        }   
    }
}
