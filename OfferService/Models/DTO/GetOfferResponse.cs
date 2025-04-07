namespace OfferService.Models.DTO
{
    public class GetOfferResponse
    {
        public string Id { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public double Price { get; set; }
        public string Datetime { get; set; }
        public string Duration { get; set; }
        public string Airline { get; set; }
        public int SeatsFirstClass { get; set; }
        public int SeatsEconomy { get; set; }
    }
}
