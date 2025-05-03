using MassTransit;
using shared.Events;

namespace OfferService.Events.Consumers;

public class BookingCancelledConsumer : IConsumer<BookingCancelledEvent>
{
    private readonly Services.OfferService _offerService;

    public BookingCancelledConsumer(Services.OfferService offerService)
    {
        _offerService = offerService;
    }

    public async Task Consume(ConsumeContext<BookingCancelledEvent> context)
    {
        var booking = context.Message;
        Console.WriteLine(" [x] Received BookingCancelled {0}", booking.Id);
        await _offerService.UpdateOfferSeats(booking.OfferId, booking.FirstClassSeats, booking.SecondClassSeats);
    }
}