using payment_service.Models;

namespace payment_service.Services
{
    public interface IPaymentService
    {
        public Task<IEnumerable<Payment>> GetPayments();
        public Task<Payment> GetPayment(string id);
        public Task Create(Payment payment);
        public Task<Payment> ProcessPayment(string bookingId);
        public Task ExpirePayment(string bookingId);
        public Task CancelPayment(string bookingId);
    }
}
