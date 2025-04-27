using MassTransit;
using OfferService.Events.Models;

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
        var offer = await _offerService.GetOfferById(booking.OfferId);

        if (offer != null)
        {
            offer.FirstClassSeats += booking.FirstClassSeats;
            offer.SecondClassSeats += booking.SecondClassSeats;
            await _offerService.UpdateOffer(offer);
        }
    }
}