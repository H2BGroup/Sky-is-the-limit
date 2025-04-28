using MassTransit;
using payment_service.Models;
using payment_service.Services;
using shared.Events;

namespace payment_service.Events
{
    public class BookingAvailableConsumer : IConsumer<BookingAvailableEvent>
    {
        private readonly IPaymentService _paymentService;

        public BookingAvailableConsumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Consume(ConsumeContext<BookingAvailableEvent> context)
        {
            var message = context.Message;

            var payment = new Payment
            {
                BookingId = message.Id,
                Value = message.Price,
                Status = PaymentStatus.Pending,
                DateOfPayment = DateTime.UtcNow
            };

             Console.WriteLine($"Received event: {context.Message.Id}");

            await _paymentService.Create(payment);

        }
    }
}
