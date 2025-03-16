namespace reservation_service.Models.DTO;

public class GetReservationResponse
{
    public class SimpleUser
    {
        required public string Id { get; set; }
        public string? Login { get; set; }
    }

    public class SimpleOffer
    {
        required public string Id { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime DepartureDate { get; set; }
    }

    required public string Id { get; set; }
    public SimpleOffer? Offer { get; set; }
    public SimpleUser? User { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public int RegisteredBaggage { get; set; }
    public double Price { get; set; }
    public ReservationStatus Status { get; set; }
}

public class GetReservationsResponse
{
    public class SimpleReservation
    {
        required public string Id { get; set; }
    }
    public IEnumerable<SimpleReservation> Reservations { get; set; } = [];
}

public class PutReservationRequest
{
    public required string OfferId { get; set; }
    public required string UserId { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public int RegisteredBaggage { get; set; }
    public double Price { get; set; }
}
