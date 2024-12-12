
using EventManagement.Data.Context;
using EventManagement.Data.Models;
using EventManagement.Data.Repository;
using EventManagement.Data.Repository.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.API.Service
{
    public class NotificationSender : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        

        public NotificationSender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                var tomorrow = DateTime.Now.AddDays(1).Date;
                using (var _contextEventRepo = _serviceProvider.CreateScope())
                {
                    var _eventRepository = _contextEventRepo.ServiceProvider.GetRequiredService<IEventRepo>();

                    var events = await _eventRepository.GetEventsWithRsvpsWithPositiveStatus(tomorrow);

                    using (var _contextSendService = _serviceProvider.CreateScope())
                    {
                        var _sender = _contextSendService.ServiceProvider.GetRequiredService<ISendService>();
                        foreach (var @event in events)
                        {
                            foreach (var rsvp in @event.Rsvps)
                            {
                                await _sender.NotificationEmail(rsvp, @event);
                            }
                        }
                    }                    
                }   
            }
        }
    }
}
