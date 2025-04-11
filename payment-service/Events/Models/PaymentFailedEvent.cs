namespace payment_service.Events.Models
{
    public class PaymentFailedEvent
    {
        required public string Id { get; set; }
        required public string BookingId { get; set; }
        required public double Value { get; set; }
        public bool Status { get; set; }
        public DateTime DateOfPayment { get; set; }
    }
}
