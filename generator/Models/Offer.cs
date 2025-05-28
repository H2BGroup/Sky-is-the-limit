using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Generator.Models
{
    public class Offer
    {
        public string Id { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public DateTime Datetime { get; set; }
        public string Duration { get; set; }
        public string Airline { get; set; }
        public int SeatsFirstClass { get; set; }
        public int SeatsEconomy { get; set; }
        public double Price { get; set; }
    }
}
