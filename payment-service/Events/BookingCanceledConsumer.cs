using MassTransit;
using payment_service.Models;
using payment_service.Services;
using shared.Events;

namespace payment_service.Events
{
    public class BookingCanceledConsumer : IConsumer<BookingCancelledEvent>
    {
        private readonly IPaymentService _paymentService;

        public BookingCanceledConsumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Consume(ConsumeContext<BookingCancelledEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"Received event: {context.Message.Id}");

            await _paymentService.CancelPayment(message.Id);

        }
    }
}
