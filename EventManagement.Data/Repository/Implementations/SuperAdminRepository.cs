﻿using AutoMapper;
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
    public class SuperAdminRepository : ISuperAdmin
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SuperAdminRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserResponseDTO?> AssignRole(AddUserRoleDTO addUserRole)
        {
            var user = await _context.Users.FindAsync(addUserRole.UserId);
            var role = await _context.Roles.FindAsync(addUserRole.RoleId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            user.RoleId = addUserRole.RoleId;
            int count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                throw new Exception("Error assigning role to user.");
            }

            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<bool> CreateRole(RoleRequestDTO role)
        {
            var newRole = _mapper.Map<Role>(role);

            await _context.AddAsync(newRole);
            int count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                throw new Exception("Error creating role.");
            }

            return true; // If save was successful, return true.
        }

    }
}
