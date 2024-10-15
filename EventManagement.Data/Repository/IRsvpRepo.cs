using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public interface IRsvpRepo
    {
        Task<int> CreateRsvps(List<Rsvp> rsvpsDto);
        Task<bool> DeleteRsvp(int rsvpId);
        Task<List<RsvpResponseDTO>> GetRsvpsByEventId(int eventId);
        Task<RsvpResponseDTO> UpdateRsvp(int rsvpId, UpdateRsvpRequestDTO rsvpDto);

        public Task<RsvpResponseDTO> UpdateRsvpStatus(int rsvpId, string status);
    }
}
