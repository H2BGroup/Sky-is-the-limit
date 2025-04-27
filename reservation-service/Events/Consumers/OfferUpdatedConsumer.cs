using MassTransit;
using reservation_service.Models;
using reservation_service.Services;
using shared.Events;

namespace reservation_service.Events.Consumers;

public class OfferUpdatedConsumer : IConsumer<OfferUpdatedEvent>
{
    private readonly IOfferService _offerService;

    public OfferUpdatedConsumer(IOfferService offerService)
    {
        _offerService = offerService;
    }

    public async Task Consume(ConsumeContext<OfferUpdatedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine(" [x] Received OfferUpdatedEvent {0}", message.Id);
        Offer? offer = _offerService.Get(message.Id);
        if (offer != null)
        {
            offer.Origin = message.Origin;
            offer.Destination = message.Destination;
            offer.DepartureDate = message.DepartureDate;
            _offerService.Update(offer);
            Console.WriteLine(" [x] Updated Offer {0}", offer.Id);
        }
        else
        {
            Console.WriteLine(" [x] Offer not found {0}", message.Id);
        }
    }
}