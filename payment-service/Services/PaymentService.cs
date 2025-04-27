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
            return await _payments.Find(x => x.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task Create(Payment payment)
        {
            await _payments.InsertOneAsync(payment);
        }

        public async Task<Payment> ProcessPayment(string bookingId)
        {
            Random rnd = new Random();
            var payment = await _payments.Find(x => x.BookingId == bookingId).FirstOrDefaultAsync();

            if (payment is null)
            {
                throw new KeyNotFoundException($"Payment with bookingId {bookingId} not found.");
            }
            
            if(payment.Status != PaymentStatus.Pending)
            {
                throw new InvalidOperationException($"Payment with bookingId {bookingId} is already processed.");
            }

            if (rnd.Next(10) == 0)
            {
                payment.Status = PaymentStatus.Failed;
                payment.DateOfPayment = DateTime.UtcNow;

                return payment;
            }
            else
            {
                payment.Status = PaymentStatus.Succeeded;
                payment.DateOfPayment = DateTime.UtcNow;

                await _payments.ReplaceOneAsync(x => x.BookingId == bookingId, payment);

                await _publisher.PublishPaymentSucceededEvent(payment.BookingId);

                return payment;
            }
        }

        public async Task ExpirePayment(string bookingId)
        {
            var payment = await _payments.Find(x => x.BookingId == bookingId).FirstOrDefaultAsync();

            if (payment is null)
            {
                throw new KeyNotFoundException($"Payment with bookingId {bookingId} not found.");
            }

            payment.Status = PaymentStatus.Failed;
            payment.DateOfPayment = DateTime.UtcNow;
            await _payments.ReplaceOneAsync(x => x.BookingId == bookingId, payment);
        }

        public async Task CancelPayment(string bookingId)
        {
            var payment = await _payments.Find(x => x.BookingId == bookingId).FirstOrDefaultAsync();

            if (payment is null)
            {
                throw new KeyNotFoundException($"Payment with bookingId {bookingId} not found.");
            }

            var refund = new Payment
            { 
                BookingId = payment.BookingId,
                Value = -payment.Value,
                Status = PaymentStatus.Succeeded,
                DateOfPayment = DateTime.UtcNow
            };

            await _payments.InsertOneAsync(refund);
        }
    }
}
