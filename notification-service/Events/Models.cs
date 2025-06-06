namespace shared.Events;

public class BookingConfirmedEvent
{
    required public string Id { get; set; }
    required public string OfferId { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public int RegisteredBaggage { get; set; }
    public int CarryOnBaggage { get; set; }
    public bool PriorityBoarding { get; set; }
    public bool Insurance { get; set; }
    public double Price { get; set; }
}

public class OfferUpdatedEvent
{
    required public string Id { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public DateTime DepartureDate { get; set; }
    public TimeSpan FlightLength { get; set; }
    public string? Airline { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public double Price { get; set; }
}