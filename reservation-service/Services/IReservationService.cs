using reservation_service.Models;

namespace reservation_service.Services;

public interface IReservationService
{
    public IEnumerable<Reservation> GetReservations();
    public Reservation? GetReservation(string id);
    public void Create(Reservation offer);
    public void Delete(string id);
}
