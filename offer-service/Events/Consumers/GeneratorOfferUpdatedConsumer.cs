using MassTransit;
using shared.Events;

namespace OfferService.Events.Consumers;

public class GeneratorOfferUpdatedConsumer : IConsumer<GeneratorOfferUpdatedEvent>
{
    private readonly Services.OfferService _offerService;
    private readonly Publisher _publisher;

    public GeneratorOfferUpdatedConsumer(Services.OfferService offerService, Publisher publisher)
    {
        _offerService = offerService;
        _publisher = publisher;
    }

    public async Task Consume(ConsumeContext<GeneratorOfferUpdatedEvent> context)
    {
        var updatedData = context.Message;
        Console.WriteLine($" [x] Received GeneratorOfferUpdated {updatedData.Id}");
        var offerToUpdate = await _offerService.GetOfferById(updatedData.Id);
        if (offerToUpdate == null)
        {
            Console.WriteLine($" [x] Offer with {updatedData.Id} id not found.");
            return;
        }
        offerToUpdate.FirstClassSeats = updatedData.FirstClassSeats;
        offerToUpdate.SecondClassSeats = updatedData.SecondClassSeats;
        offerToUpdate.Price = updatedData.Price;
        await _offerService.UpdateOffer(offerToUpdate);

        Console.WriteLine($" [x] Offer {offerToUpdate.Id} updated.");
    }
}