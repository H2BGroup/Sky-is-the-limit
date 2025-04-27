namespace OfferService.Events.Models;

public class BookingCancelledEvent
{
    required public string Id { get; set; }
    required public string OfferId { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
}