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

        public async Task PublishPaymentSucceededEvent(string bookingId)
        {
            var paymentSucceededEvent = new PaymentSucceededEvent
            {
                BookingId = bookingId
            };

            await _publishEndpoint.Publish(paymentSucceededEvent);
        }
    }
}
