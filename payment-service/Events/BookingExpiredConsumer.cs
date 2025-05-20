using MassTransit;
using payment_service.Models;
using payment_service.Services;
using shared.Events;

namespace payment_service.Events
{
    public class BookingExpiredConsumer : IConsumer<BookingExpiredEvent>
    {
        private readonly IPaymentService _paymentService;

        public BookingExpiredConsumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Consume(ConsumeContext<BookingExpiredEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"Received event: {context.Message.Id}");

            await _paymentService.ExpirePayment(message.Id);
        }
    }
}
