using System.ComponentModel.DataAnnotations;

namespace reservation_service.Models;

public class Offer
{
    [Key]
    required public string Id { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public DateTime DepartureDate { get; set; }

    public List<Reservation> Reservations { get; set; } = new();
}
