
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OfferService.Models.DomainEvents;

public class OfferSeatsUpdatedEvent : DomainEvent
{
    public string Id { get; set; }
    public int FirstClassSeatsDiff { get; set; }
    public int SecondClassSeatsDiff { get; set; }
}

public class OfferSeatsUpdatedEventHandler : INotificationHandler<OfferSeatsUpdatedEvent>
{
    private readonly OfferContext _context;

    public OfferSeatsUpdatedEventHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task Handle(OfferSeatsUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == notification.Id);
        if (offer != null)
        {
            offer.FirstClassSeats += notification.FirstClassSeatsDiff;
            offer.SecondClassSeats += notification.SecondClassSeatsDiff;
            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
        }
    }
}