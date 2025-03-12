using Microsoft.EntityFrameworkCore;

namespace reservation_service.Models;

public class ReservationContext : DbContext
{
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<User> Users { get; set; }

    public ReservationContext(DbContextOptions<ReservationContext> options)
        : base(options)
    {
    }
}
