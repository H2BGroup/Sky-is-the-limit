using MediatR;
using Microsoft.EntityFrameworkCore;
using OfferService.Models;

namespace OfferService.Queries;

public record GetOffersQuerry() : IRequest<IEnumerable<Offer>>;

public class GetOffersQuerryHandler : IRequestHandler<GetOffersQuerry, IEnumerable<Offer>>
{
    private readonly OfferContext _context;

    public GetOffersQuerryHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Offer>> Handle(GetOffersQuerry request, CancellationToken cancellationToken)
    {
        return await _context.Offers.ToListAsync();
    }
}