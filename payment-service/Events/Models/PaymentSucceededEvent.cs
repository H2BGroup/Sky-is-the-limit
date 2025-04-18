namespace payment_service.Events.Models
{
    public class PaymentSucceededEvent
    {
        required public string BookingId { get; set; }
    }
}
