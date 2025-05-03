using MediatR;
using Microsoft.EntityFrameworkCore;
using OfferService.Models;

namespace OfferService.Queries;

public record GetOfferByIdQuery(string id) : IRequest<Offer?>;

public class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQuery, Offer?>
{
    private readonly OfferContext _context;

    public GetOfferByIdQueryHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task<Offer?> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Offers.FirstOrDefaultAsync(o => o.Id == request.id);
    }
}