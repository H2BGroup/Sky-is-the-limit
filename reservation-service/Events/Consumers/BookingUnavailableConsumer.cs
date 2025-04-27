using MassTransit;
using reservation_service.Models;
using reservation_service.Services;

namespace reservation_service.Events.Consumers;

public class BookingUnavailableConsumer : IConsumer<BookingUnavailableEvent>
{
    private readonly IBookingService _bookingService;

    public BookingUnavailableConsumer(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task Consume(ConsumeContext<BookingUnavailableEvent> context)
    {
        var message = context.Message;
        Console.WriteLine(" [x] Received BookingUnavailable {0}", message.Id);
        _bookingService.Delete(message.Id);
        Console.WriteLine(" [x] Deleted Booking {0}", message.Id);
    }
}