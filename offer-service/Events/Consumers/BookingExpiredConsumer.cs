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
        var offer = await _offerService.GetOfferById(booking.OfferId);

        if (offer != null)
        {
            offer.FirstClassSeats += booking.FirstClassSeats;
            offer.SecondClassSeats += booking.SecondClassSeats;
            await _offerService.UpdateOffer(offer);
        }
    }
}