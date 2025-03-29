using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace payment_service.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        required public string Id { get; set; }
        public double Value { get; set; }
        public bool Status { get; set; }
        public DateTime DateOfPayment { get; set; }
    }
}
