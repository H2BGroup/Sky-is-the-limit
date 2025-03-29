using Microsoft.Extensions.Options;
using MongoDB.Driver;
using payment_service.Data;
using payment_service.Models;

namespace payment_service.Services
{
    public class PaymentService:IPaymentService
    {
        private readonly IMongoCollection<Payment> _payments;

        public PaymentService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(
                mongoDBSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDBSettings.Value.DatabaseName);

            _payments = mongoDatabase.GetCollection<Payment>(
                mongoDBSettings.Value.PaymentsCollectionName);
        }

        public async Task Create(Payment payment) =>
            await _payments.InsertOneAsync(payment);

        public async Task<Payment?> GetPayment(string id) =>
           await _payments.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Payment>> GetPayments() =>
            await _payments.Find(_ => true).ToListAsync();
    }
}
