namespace reservation_service.Models;

public class Offer
{
    required public string Id { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public DateTime DepartureDate { get; set; }
}
