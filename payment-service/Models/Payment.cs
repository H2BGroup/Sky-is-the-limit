using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace payment_service.Models
{
    public class Payment
    {
        [BsonId]
        public ObjectId Id { get; set; }
        required public string BookingId { get; set; }
        required public double Value { get; set; }
        public bool Status { get; set; }
        public DateTime DateOfPayment { get; set; }
    }
}
