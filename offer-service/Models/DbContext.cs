using Microsoft.EntityFrameworkCore;

namespace OfferService.Models
{
    public class OfferContext : DbContext
    {
        public OfferContext(DbContextOptions<OfferContext> options) : base(options)
        {
        }

        public DbSet<Offer> Offers { get; set; }

    }
}
