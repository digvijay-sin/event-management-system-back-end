
using EventManagement.Data.Repository;

namespace EventManagement.API.Service
{
    public class EventStatusService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EventStatusService(IServiceProvider serviceProvider)
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

                    await _eventRepository.SetFinishedEvents(stoppingToken);

                    await _eventRepository.SetOngoingEvents(stoppingToken);

                    await _eventRepository.SetUpcomingEvents(stoppingToken);
                }
            }
        }
    }
}
