using reservation_service.Models;

namespace reservation_service.Services;

public interface IBookingService
{
    public IEnumerable<Booking> GetBookings();
    public Booking? GetBooking(string id);
    public void Create(Booking booking);
    public void Delete(string id);
    public void Update(Booking booking);
}
