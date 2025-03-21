using Microsoft.EntityFrameworkCore;

namespace reservation_service.Models;

public class ReservationContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<User> Users { get; set; }

    public ReservationContext(DbContextOptions<ReservationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .HasOne(r => r.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Booking>()
            .HasOne(r => r.Offer)
            .WithMany(o => o.Bookings)
            .HasForeignKey(r => r.OfferId);
    }
}
