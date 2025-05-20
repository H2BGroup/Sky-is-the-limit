using Microsoft.EntityFrameworkCore;
using OfferService.Models.DomainEvents;

namespace OfferService.Models
{
    public class OfferContext : DbContext
    {
        public OfferContext(DbContextOptions<OfferContext> options) : base(options)
        {
        }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<DomainEvent> DomainEvents { get; set; }
    }
}
