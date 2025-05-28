namespace shared.Events;

public class GeneratorOfferUpdatedEvent
{
    required public string Id { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public double Price { get; set; }
}

