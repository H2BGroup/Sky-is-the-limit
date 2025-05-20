using reservation_service.Events;
using reservation_service.Models;
using shared.Events;

namespace reservation_service.Services;

public class BookingExpirationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);
    private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(1);

    public BookingExpirationWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
                var publisher = scope.ServiceProvider.GetRequiredService<Publisher>();

                var now = DateTime.UtcNow;
                var bookings = bookingService.GetCurrentReservations()
                    .Where(b => (now - b.StatusTime) >= _expirationTime).ToList();

                foreach (var booking in bookings)
                {
                    Console.WriteLine(" [x] Booking Expired {0}", booking.Id);
                    booking.Status = BookingStatus.Cancelled;
                    booking.StatusTime = now;
                    bookingService.Update(booking);
                    Console.WriteLine(" [x] Updated Booking Status {0}", booking.Id);
                    await publisher.Publish(new BookingExpiredEvent
                    {
                        Id = booking.Id,
                        OfferId = booking.OfferId,
                        FirstClassSeats = booking.FirstClassSeats,
                        SecondClassSeats = booking.SecondClassSeats
                    });
                    Console.WriteLine(" [x] Published BookingExpiredEvent {0}", booking.Id);
                }
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}