using MediatR;

namespace OfferService.Models.DomainEvents;

public class OfferCreatedEvent : DomainEvent
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

public class OfferCreatedEventHandler : INotificationHandler<OfferCreatedEvent>
{
    private readonly OfferContext _context;

    public OfferCreatedEventHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task Handle(OfferCreatedEvent notification, CancellationToken cancellationToken)
    {
        var offer = new Offer
        {
            Id = notification.Id,
            Origin = notification.Origin,
            Destination = notification.Destination,
            DepartureDateTime = notification.DepartureDateTime,
            FlightLength = notification.FlightLength,
            Airline = notification.Airline,
            FirstClassSeats = notification.FirstClassSeats,
            SecondClassSeats = notification.SecondClassSeats,
            Price = notification.Price
        };

        _context.Offers.Add(offer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}