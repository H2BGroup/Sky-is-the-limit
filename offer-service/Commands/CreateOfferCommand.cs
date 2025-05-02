using MediatR;
using OfferService.Models;

namespace OfferService.Commands;

public record CreateOfferCommand(Offer offer) : IRequest;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand>
{
    private readonly OfferContext _context;

    public CreateOfferCommandHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        _context.Offers.Add(request.offer);
        await _context.SaveChangesAsync();
    }
}