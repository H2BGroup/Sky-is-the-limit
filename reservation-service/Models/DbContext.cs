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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Offer)
            .WithMany(o => o.Reservations)
            .HasForeignKey(r => r.OfferId);
    }
}
