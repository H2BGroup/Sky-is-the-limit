using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using payment_service.Data;
using payment_service.Events;
using payment_service.Models;

namespace payment_service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMongoCollection<Payment> _payments;
        private readonly Publisher _publisher;

        public PaymentService(IOptions<MongoDBSettings> mongoDBSettings, Publisher publisher)
        {
            var mongoClient = new MongoClient(
                mongoDBSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDBSettings.Value.DatabaseName);

            _payments = mongoDatabase.GetCollection<Payment>(
                mongoDBSettings.Value.PaymentsCollectionName);

            _publisher = publisher;
        }

        public async Task<IEnumerable<Payment>> GetPayments() =>
            await _payments.Find(_ => true).ToListAsync();

        public async Task<Payment> GetPayment(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;
            return await _payments.Find(x => x.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task Create(Payment payment)
        {
            Random rnd = new Random();

            if (rnd.Next(10) == 0)
            {
                return;
            }
            else
            {
                await _payments.InsertOneAsync(payment);
                await _publisher.PublishPaymentSucceededEvent(payment.BookingId);

                return;
            }
        }

    }
}
