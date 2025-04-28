using reservation_service.Models;
using Microsoft.EntityFrameworkCore;

namespace reservation_service.Services;

public class OfferService : IOfferService
{
    private readonly ReservationContext _context;

    public OfferService(ReservationContext context)
    {
        _context = context;
    }

    public Offer? Get(string id)
    {
        return _context.Offers.FirstOrDefault(r => r.Id == id);
    }

    public void Create(Offer offer)
    {
        _context.Offers.Add(offer);
        _context.SaveChanges();
    }

    public void Update(Offer offer)
    {
        _context.Offers.Update(offer);
        _context.SaveChanges();
    }
}