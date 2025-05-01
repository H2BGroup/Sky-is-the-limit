using reservation_service.Models;
using Microsoft.EntityFrameworkCore;

namespace reservation_service.Services;

public class BookingService : IBookingService
{
    private readonly ReservationContext _context;

    public BookingService(ReservationContext context)
    {
        _context = context;
    }

    public void Create(Booking booking)
    {
        if(_context.Offers.Find(booking.OfferId) == null)
        {
            throw new ArgumentException("Offer not found");
        }
        if(_context.Users.Find(booking.UserId) == null)
        {
            throw new ArgumentException("User not found");
        }
        _context.Bookings.Add(booking);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var booking = _context.Bookings.Find(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
        }
    }

    public Booking? GetBooking(string id)
    {
        return _context.Bookings.Include(r => r.User).Include(r => r.Offer).FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Booking> GetBookings()
    {
        return _context.Bookings.Include(r => r.User).Include(r => r.Offer).ToList();
    }

    public void Update(Booking booking)
    {
        if(_context.Offers.Find(booking.OfferId) == null)
        {
            throw new ArgumentException("Offer not found");
        }
        if(_context.Users.Find(booking.UserId) == null)
        {
            throw new ArgumentException("User not found");
        }
        _context.Bookings.Update(booking);
        _context.SaveChanges();
    }

    public IEnumerable<Booking> GetCurrentReservations()
    {
        return _context.Bookings.Where(b => b.Status == BookingStatus.Reserved).ToList();
    }
}