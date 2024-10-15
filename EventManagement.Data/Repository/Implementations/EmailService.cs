using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventManagement.Data.Models;
using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Context;

namespace EventManagement.Data.Repository.Implementations
{
    public class EmailService : ISendService
    {
        private readonly string FromEmail = "digvijaysingh@sageuniversity.in";
        private readonly ApplicationDbContext _context;

        public EmailService(ApplicationDbContext context)
        {
            _context = context;
        }
        

        private string InvitationMailBody(int rsvpId, EventResponseDTO @event) { 
            return $@"            
            <html>
            <head>
                <title>RSVP for {@event.Title}</title>
            </head>
            <body>
                <h2>RSVP for {@event.Title}</h2>
                <p>You have been invited to the event: <strong>{@event.Title} held at location {@event.Location} on {@event.EventDate}</strong>.</p>
                <p>Please click on one of the options below to let us know your response:</p>
                <a href='https://Localhost:7226/api/Rsvp/respond?status=going&rsvpId={rsvpId}' style='padding: 10px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px; margin-right: 5px;'>Going</a>
                <a href='https://Localhost:7226/api/Rsvp/respond?status=maybe&rsvpId={rsvpId}' style='padding: 10px; background-color: #ffc107; color: white; text-decoration: none; border-radius: 5px; margin-right: 5px;'>Maybe</a>
                <a href='https://Localhost:7226/api/Rsvp/respond?status=notgoing&rsvpId={rsvpId}' style='padding: 10px; background-color: #dc3545; color: white; text-decoration: none; border-radius: 5px;'>Not Going</a>
                <p>Thank you!</p>
                <p>Best regards,<br>Dev's Event Management</p>
            </body>
            </html>";
        }

        public async Task InvitationEMail(RsvpResponseDTO rsvp)
        {            
           
            var message = new MailMessage
            {
                From = new MailAddress(FromEmail),
                To = { rsvp.Email },
                Subject = $"RSVP For {rsvp.Event.Title}",
                Body = InvitationMailBody(rsvp.RsvpId, rsvp.Event),
                IsBodyHtml = true
            };

            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.Credentials = new NetworkCredential(FromEmail, "sage@1234");
                client.EnableSsl = true;

                await client.SendMailAsync(message);
            }
        }

        private string NotificationMailBody(EventWithRsvpsDTO @event) {
            return $@"
             <html>
                <head>
                    <title>A Gentle Reminder for attending the Event Tomorrow at {@event.Location} on {@event.EventDate.TimeOfDay}</title>
                </head>
                <body>
                    <h2>Your Wait is Over!</h2>
                    <p>Be Ready to Join <strong>Tomorrow</strong>the Event {@event.Title},full of enthusiasm, joy and Learning</p>                   
                    <p>Best regards,<br>Dev's Event Management</p>
                </body>
            </html>";
        }

        public async Task NotificationEmail(RsvpResponseDTO rsvp, EventWithRsvpsDTO @event )
        {
            var message = new MailMessage()
            {
                From = new MailAddress(FromEmail),
                To = { rsvp.Email },
                Subject = $"A Gentle Reminder",
                Body = NotificationMailBody(@event),
                IsBodyHtml = true
            };

            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                client.Credentials = new NetworkCredential();
                client.EnableSsl = true;

                await client.SendMailAsync(message);
            }
        }
    }
}
