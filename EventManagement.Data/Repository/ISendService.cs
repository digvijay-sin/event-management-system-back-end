using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public interface ISendService
    {
        public Task InvitationEMail(RsvpResponseDTO rsvp);

        public Task NotificationEmail(RsvpResponseDTO rsvp, EventWithRsvpsDTO @event);
    }
}
