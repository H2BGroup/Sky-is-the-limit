namespace payment_service.Data
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string PaymentsCollectionName { get; set; } = null!;
    }
}
