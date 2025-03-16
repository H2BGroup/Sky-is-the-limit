using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_service.Models;

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled
}

public class Reservation
{
    [Key]
    required public string Id { get; set; }

    [ForeignKey("Offer")]
    public required string OfferId { get; set; }
    public Offer? Offer { get; set; }

    [ForeignKey("User")]
    public required string UserId { get; set; }
    public User? User { get; set; }

    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public int RegisteredBaggage { get; set; }
    public double Price { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReservationStatus Status { get; set; }
}
