using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OfferService.Models.DomainEvents;

public class OfferUpdatedEvent : DomainEvent
{
    public string Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public TimeSpan FlightLength { get; set; }
    public string Airline { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public double Price { get; set; }
}

public class OfferUpdatedEventHandler : INotificationHandler<OfferUpdatedEvent>
{
    private readonly OfferContext _context;

    public OfferUpdatedEventHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task Handle(OfferUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == notification.Id);
        if (offer != null)
        {
            offer.Origin = notification.Origin;
            offer.Destination = notification.Destination;
            offer.DepartureDateTime = notification.DepartureDateTime;
            offer.FlightLength = notification.FlightLength;
            offer.Airline = notification.Airline;
            offer.FirstClassSeats = notification.FirstClassSeats;
            offer.SecondClassSeats = notification.SecondClassSeats;
            offer.Price = notification.Price;

            _context.Offers.Update(offer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}