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
    public class RsvpRepository : IRsvpRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RsvpRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateRsvps(List<Rsvp> rsvpsDto)
        {
            foreach (var rsvp in rsvpsDto)
            {
                await _context.Rsvps.AddAsync(rsvp);
            }

            int count = await _context.SaveChangesAsync();

            if (count == 0)
            {
                throw new Exception("Error creating RSVP");
            }

            return count;
        }

        public async Task<bool> DeleteRsvp(int rsvpId)
        {
            var rsvp = await _context.Rsvps.FindAsync(rsvpId);
            if (rsvp == null)
            {
                throw new Exception("RSVP not found");
            }

            _context.Rsvps.Remove(rsvp);
            int count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                throw new Exception("Error deleting RSVP");
            }

            return true;
        }

        public async Task<List<RsvpResponseDTO>> GetRsvpsByEventId(int eventId)
        {
            var rsvps = await _context.Rsvps.Where(r => r.EventId == eventId).Include(r => r.Event).ToListAsync();
            if (rsvps.Count == 0)
            {
                throw new Exception("No RSVPs found for this event.");
            }
            return _mapper.Map<List<RsvpResponseDTO>>(rsvps);
        }

        public async Task<RsvpResponseDTO> UpdateRsvp(int rsvpId, UpdateRsvpRequestDTO rsvpDto)
        {
            var rsvp = await _context.Rsvps.FindAsync(rsvpId);
            if (rsvp == null)
            {
                throw new Exception("RSVP not found");
            }

            _context.Entry(rsvp).CurrentValues.SetValues(rsvpDto);
            _context.Entry(rsvp).State = EntityState.Modified;

            int count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                throw new Exception("Error updating RSVP");
            }

            return _mapper.Map<RsvpResponseDTO>(rsvp);
        }

        public async Task<RsvpResponseDTO> UpdateRsvpStatus(int rsvpId, string status)
        {
            var rsvp = await _context.Rsvps.FindAsync(rsvpId);
            if (rsvp == null)
            {
                throw new Exception("RSVP not found");
            }

            rsvp.Status = status;
            _context.Entry(rsvp).State = EntityState.Modified;

            int count = await _context.SaveChangesAsync();
            if (count == 0)
            {
                throw new Exception("Error updating RSVP status");
            }

            return _mapper.Map<RsvpResponseDTO>(rsvp);
        }

    }
}
