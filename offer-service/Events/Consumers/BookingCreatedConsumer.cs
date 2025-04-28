using MassTransit;
using shared.Events;

namespace OfferService.Events.Consumers;

public class BookingCreatedConsumer : IConsumer<BookingCreatedEvent>
{
    private readonly Services.OfferService _offerService;
    private readonly Publisher _publisher;

    public BookingCreatedConsumer(Services.OfferService offerService, Publisher publisher)
    {
        _offerService = offerService;
        _publisher = publisher;
    }

    public async Task Consume(ConsumeContext<BookingCreatedEvent> context)
    {
        var booking = context.Message;
        Console.WriteLine(" [x] Received BookingCreated {0}", booking.Id);
        var offer = await _offerService.GetOfferById(booking.OfferId);

        if (offer!=null && offer.FirstClassSeats >= booking.FirstClassSeats && offer.SecondClassSeats >= booking.SecondClassSeats)
        {
            offer.FirstClassSeats -= booking.FirstClassSeats;
            offer.SecondClassSeats -= booking.SecondClassSeats;
            await _offerService.UpdateOffer(offer);

            await _publisher.Publish(new BookingAvailableEvent
            {
                Id = booking.Id,
                OfferId = booking.OfferId,
                FirstClassSeats = booking.FirstClassSeats,
                SecondClassSeats = booking.SecondClassSeats,
                Price = booking.Price
            });
            Console.WriteLine(" [x] Published BookingAvailable {0}", booking.Id);
        }
        else
        {
            await _publisher.Publish(new BookingUnavailableEvent
            {
                Id = booking.Id,
                OfferId = booking.OfferId,
                FirstClassSeats = booking.FirstClassSeats,
                SecondClassSeats = booking.SecondClassSeats
            });
            Console.WriteLine(" [x] Published BookingUnavailable {0}", booking.Id);
        }
    }
}