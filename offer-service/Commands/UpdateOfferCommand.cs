using MediatR;
using OfferService.Models;

namespace OfferService.Commands;

public record UpdateOfferCommand(Offer offer) : IRequest;

public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
{
    private readonly OfferContext _context;

    public UpdateOfferCommandHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
    {
        _context.Offers.Update(request.offer);
        await _context.SaveChangesAsync();
    }
}