namespace reservation_service.Models.DTO;

public class GetBookingResponse
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
    public int CarryOnBaggage { get; set; }
    public bool PriorityBoarding { get; set; }
    public bool Insurance { get; set; }
    public double Price { get; set; }
    public BookingStatus Status { get; set; }
}

public class GetBookingsResponse
{
    public class SimpleBooking
    {
        required public string Id { get; set; }
    }
    public IEnumerable<SimpleBooking> Bookings { get; set; } = [];
}

public class PutBookingRequest
{
    public required string OfferId { get; set; }
    public required string UserId { get; set; }
    public int FirstClassSeats { get; set; }
    public int SecondClassSeats { get; set; }
    public int RegisteredBaggage { get; set; }
    public int CarryOnBaggage { get; set; }
    public bool PriorityBoarding { get; set; }
    public bool Insurance { get; set; }
    public double Price { get; set; }
}
