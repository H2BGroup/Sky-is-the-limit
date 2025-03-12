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
    public string Id { get; set; }
    public Offer Offer { get; set; }
    public User User { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public double Price { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReservationStatus Status { get; set; }
}
