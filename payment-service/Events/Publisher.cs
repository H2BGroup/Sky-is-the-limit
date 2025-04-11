using MassTransit;
using payment_service.Events.Models;

namespace payment_service.Events
{
    public class Publisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public Publisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishPaymentFailedEvent(string paymentId, string bookingId, double value)
        {
            var paymentFailedEvent = new PaymentFailedEvent
            {
                Id = paymentId,
                BookingId = bookingId,
                Value = value,
                Status = false,
                DateOfPayment = DateTime.UtcNow
            };

            await _publishEndpoint.Publish(paymentFailedEvent);
        }
    }
}
