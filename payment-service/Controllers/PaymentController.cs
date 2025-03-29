using Microsoft.AspNetCore.Mvc;
using payment_service.Models;
using payment_service.Services;

namespace payment_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService) =>
            _paymentService = paymentService;

        [HttpGet]
        public async Task<List<Payment>> Get() =>
            await _paymentService.GetPayments();

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> Get(string id)
        {
            var payment = await _paymentService.GetPayment(id);

            if (payment is null)
            {
                return NotFound();
            }

            return payment;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Payment payment)
        {
            await _paymentService.Create(payment);

            return CreatedAtAction(nameof(Get), new { id = payment.Id }, payment);
        }

    }
}