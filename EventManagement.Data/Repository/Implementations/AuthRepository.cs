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
            try
            {

                var user = await _context.Users
                     .Include(u => u.Role)
                     .FirstOrDefaultAsync(u => u.Email == creds.Email);


                if (user == null)
                {
                    return null;
                }


                if (user.Password == creds.Password)
                {
                    return _mapper.Map<UserResponseDTO>(user);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while authenticating the user.", ex);
            }
        }
    }
}
