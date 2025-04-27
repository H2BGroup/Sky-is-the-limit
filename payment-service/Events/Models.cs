namespace shared.Events;

public class BookingAvailableEvent
{
    required public string Id { get; set; }
    required public string OfferId { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public double Price { get; set; }
}

public class PaymentSucceededEvent
{
    required public string BookingId { get; set; }
}