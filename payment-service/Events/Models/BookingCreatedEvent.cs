namespace payment_service.Events.Models
{
    public class BookingCreatedEvent
    {
        required public string Id { get; set; }
        required public string OfferId { get; set; }
        public int FirstClassSeats { get; set; }
        public int SecondClassSeats { get; set; }
        public double Price { get; set; }
    }
}
