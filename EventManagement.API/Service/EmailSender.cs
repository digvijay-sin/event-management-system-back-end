using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Models;
using EventManagement.Data.Repository;

namespace EventManagement.API.Service
{
    public class EmailSender
    {
        private readonly ISendService _sender;

        public EmailSender(ISendService sender)
        {
            _sender = sender;
        }

        public async Task SendInvitationEmailAsync(List<RsvpResponseDTO> rsvps) { 

            foreach(var rsvp in rsvps)
            {                
                await _sender.InvitationEMail(rsvp);
            }
        }
      
    }
}
