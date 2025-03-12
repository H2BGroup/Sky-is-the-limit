namespace reservation_service.Models;

public class Offer
{
    public string Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureDate { get; set; }
}
