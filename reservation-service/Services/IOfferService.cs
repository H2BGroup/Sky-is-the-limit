using reservation_service.Models;

namespace reservation_service.Services;

public interface IOfferService
{
    public Offer? Get(string id);
    public void Create(Offer offer);
    public void Update(Offer offer);
}