using MassTransit;
using payment_service.Events.Models;
using payment_service.Models;
using payment_service.Services;

namespace payment_service.Events
{
    public class Consumer : IConsumer<BookingCreatedEvent>
    {
        private readonly IPaymentService _paymentService;

        public Consumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Consume(ConsumeContext<BookingCreatedEvent> context)
        {
            var message = context.Message;

            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                BookingId = message.Id,
                Value = message.Price,
                Status = true,
                DateOfPayment = DateTime.UtcNow
            };

            await _paymentService.Create(payment);

        }
    }
}
