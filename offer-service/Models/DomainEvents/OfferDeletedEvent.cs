using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OfferService.Models.DomainEvents;

public class OfferDeletedEvent : DomainEvent
{
    public string Id { get; set; }
}

public class OfferDeletedEventHandler : INotificationHandler<OfferDeletedEvent>
{
    private readonly OfferContext _context;

    public OfferDeletedEventHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task Handle(OfferDeletedEvent notification, CancellationToken cancellationToken)
    {
        var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == notification.Id);
        if (offer != null)
        {
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}