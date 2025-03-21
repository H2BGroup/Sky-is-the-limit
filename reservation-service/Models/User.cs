using System.ComponentModel.DataAnnotations;

namespace reservation_service.Models;

public class User
{
    [Key]
    required public string Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }

    public List<Booking> Bookings { get; set; } = new();
}
