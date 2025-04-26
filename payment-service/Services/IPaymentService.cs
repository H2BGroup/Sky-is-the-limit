using payment_service.Models;

namespace payment_service.Services
{
    public interface IPaymentService
    {
        public Task<IEnumerable<Payment>> GetPayments();
        public Task<Payment> GetPayment(string id);
    }
}
