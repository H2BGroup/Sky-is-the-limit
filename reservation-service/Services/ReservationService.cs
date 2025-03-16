using reservation_service.Models;
using Microsoft.EntityFrameworkCore;

namespace reservation_service.Services;

public class ReservationService : IReservationService
{
    private readonly ReservationContext _context;

    public ReservationService(ReservationContext context)
    {
        _context = context;
    }

    public void Create(Reservation reservation)
    {
        if(_context.Offers.Find(reservation.OfferId) == null)
        {
            throw new ArgumentException("Offer not found");
        }
        if(_context.Users.Find(reservation.UserId) == null)
        {
            throw new ArgumentException("User not found");
        }
        _context.Reservations.Add(reservation);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var reservation = _context.Reservations.Find(id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
        }
    }

    public Reservation? GetReservation(string id)
    {
        return _context.Reservations.Include(r => r.User).Include(r => r.Offer).FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Reservation> GetReservations()
    {
        return _context.Reservations.Include(r => r.User).Include(r => r.Offer).ToList();
    }
}