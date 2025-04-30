using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OfferService.Models
{
    public class Offer
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public DateTime DepartureDateTime { get; set; }

        [Required]
        public TimeSpan FlightLength { get; set; }

        [Required]
        public string Airline { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int FirstClassSeats { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int SecondClassSeats { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
