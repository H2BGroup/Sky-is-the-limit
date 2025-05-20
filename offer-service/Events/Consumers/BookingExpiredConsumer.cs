using MassTransit;
using shared.Events;

namespace OfferService.Events.Consumers;

public class BookingExpiredConsumer : IConsumer<BookingExpiredEvent>
{
    private readonly Services.OfferService _offerService;

    public BookingExpiredConsumer(Services.OfferService offerService)
    {
        _offerService = offerService;
    }

    public async Task Consume(ConsumeContext<BookingExpiredEvent> context)
    {
        var booking = context.Message;
        Console.WriteLine(" [x] Received BookingExpired {0}", booking.Id);
        await _offerService.UpdateOfferSeats(booking.OfferId, booking.FirstClassSeats, booking.SecondClassSeats);
    }
}