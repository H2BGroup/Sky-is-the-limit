using MassTransit;
using reservation_service.Models;
using reservation_service.Services;
using shared.Events;

namespace reservation_service.Events.Consumers;

public class BookingAvailableConsumer : IConsumer<BookingAvailableEvent>
{
    private readonly IBookingService _bookingService;

    public BookingAvailableConsumer(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task Consume(ConsumeContext<BookingAvailableEvent> context)
    {
        var message = context.Message;
        Console.WriteLine(" [x] Received BookingAvailable {0}", message.Id);
        Booking? booking = _bookingService.GetBooking(message.Id);
        if (booking != null && booking.Status == BookingStatus.Pending)
        {
            booking.Status = BookingStatus.Reserved;
            _bookingService.Update(booking);
            Console.WriteLine(" [x] Updated Booking Status {0}", booking);
        }
        //TODO: Start timer for booking expiration
    }
}