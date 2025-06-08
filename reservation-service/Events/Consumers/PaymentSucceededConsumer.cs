using MassTransit;
using reservation_service.Models;
using reservation_service.Services;
using reservation_service.Events;
using shared.Events;

namespace reservation_service.Events.Consumers;

public class PaymentSucceededConsumer : IConsumer<PaymentSucceededEvent>
{
    private readonly IBookingService _bookingService;
    private readonly Sender _sender;
    private readonly Publisher _publisher;

    public PaymentSucceededConsumer(IBookingService bookingService, Sender sender, Publisher publisher)
    {
        _bookingService = bookingService;
        _sender = sender;
        _publisher = publisher;
    }

    public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
    {
        var message = context.Message;
        Console.WriteLine(" [x] Received PaymentSucceeded {0}", message.BookingId);
        Booking? booking = _bookingService.GetBooking(message.BookingId);
        if (booking != null && booking.Status == BookingStatus.Reserved)
        {
            booking.Status = BookingStatus.Confirmed;
            booking.StatusTime = DateTime.UtcNow;
            _bookingService.Update(booking);
            Console.WriteLine(" [x] Updated Booking Status {0}", booking);
            BookingConfirmedEvent @event = new BookingConfirmedEvent
            {
                Id = booking.Id,
                OfferId = booking.OfferId,
                FirstClassSeats = booking.FirstClassSeats,
                SecondClassSeats = booking.SecondClassSeats,
                RegisteredBaggage = booking.RegisteredBaggage,
                CarryOnBaggage = booking.CarryOnBaggage,
                PriorityBoarding = booking.PriorityBoarding,
                Insurance = booking.Insurance,
                Price = booking.Price
            };
            await _sender.Send(@event, "TO-OUTPUT-QUEUE");
            await _publisher.Publish(@event);
            Console.WriteLine(" [x] Published BookingConfirmed {0}", booking.Id);
        }
    }
}