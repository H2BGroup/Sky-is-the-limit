using MediatR;
using OfferService.Models;

namespace OfferService.Commands;

public record DeleteOfferCommand(string id) : IRequest<bool>;

public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand, bool>
{
    private readonly OfferContext _context;

    public DeleteOfferCommandHandler(OfferContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _context.Offers.FindAsync(request.id);
        if (offer == null) return false;

        _context.Offers.Remove(offer);
        await _context.SaveChangesAsync();
        return true;
    }
}