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
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var payments = await _paymentService.GetPayments();

            return Ok(payments);
        }

        [HttpGet("{bookingId}")]
        public async Task<ActionResult<Payment>> GetPayment(string bookingId)
        {
            try
            {
                var payment = await _paymentService.GetPayment(bookingId);

                return Ok(payment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{bookingId}")]
        public async Task<ActionResult<Payment>> ProcessPayment(string bookingId)
        {
            try
            {
                var payment = await _paymentService.ProcessPayment(bookingId);

                return Ok(payment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(402);
            }
        }
    }
}