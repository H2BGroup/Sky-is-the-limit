using MassTransit;
using shared.Events;

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
