using MassTransit;
using reservation_service.Models;
using reservation_service.Services;
using shared.Events;

namespace reservation_service.Events.Consumers;

public class PaymentSucceededConsumer : IConsumer<PaymentSucceededEvent>
{
    private readonly IBookingService _bookingService;

    public PaymentSucceededConsumer(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
    {
        var message = context.Message;
        Console.WriteLine(" [x] Received PaymentSucceeded {0}", message.BookingId);
        Booking? booking = _bookingService.GetBooking(message.BookingId);
        if (booking != null && booking.Status == BookingStatus.Reserved)
        {
            booking.Status = BookingStatus.Confirmed;
            _bookingService.Update(booking);
            Console.WriteLine(" [x] Updated Booking Status {0}", booking);
        }
        //TODO: Send BookingConfirmed event
    }
}