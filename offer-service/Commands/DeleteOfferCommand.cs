using MediatR;
using Microsoft.EntityFrameworkCore;
using OfferService.Models;
using OfferService.Models.DomainEvents;

namespace OfferService.Commands;

public record DeleteOfferCommand(string id) : IRequest<bool>;

public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand, bool>
{
    private readonly OfferContext _context;
    private readonly IMediator _mediator;

    public DeleteOfferCommandHandler(OfferContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _context.Offers.FirstOrDefaultAsync(x => x.Id == request.id);
        if (offer == null) return false;

        var @event = new OfferDeletedEvent
        {
            Id = offer.Id,
        };

        _context.DomainEvents.Add(@event);
        await _context.SaveChangesAsync();

        await _mediator.Publish(@event);
        return true;
    }
}