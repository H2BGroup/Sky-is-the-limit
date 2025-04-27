using MassTransit;
using reservation_service.Models;
using reservation_service.Services;
using shared.Events;

namespace reservation_service.Events.Consumers;

public class OfferCreatedConsumer : IConsumer<OfferCreatedEvent>
{
    private readonly IOfferService _offerService;

    public OfferCreatedConsumer(IOfferService offerService)
    {
        _offerService = offerService;
    }

    public async Task Consume(ConsumeContext<OfferCreatedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine(" [x] Received OfferCreatedEvent {0}", message.Id);
        Offer offer = new Offer
        {
            Id = message.Id,
            Origin = message.Origin,
            Destination = message.Destination,
            DepartureDate = message.DepartureDate,
        };
        _offerService.Create(offer);
        Console.WriteLine(" [x] Created Offer {0}", offer.Id);
    }
}