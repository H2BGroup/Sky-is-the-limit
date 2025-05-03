using MediatR;
using Microsoft.EntityFrameworkCore;
using OfferService.Models;
using OfferService.Models.DomainEvents;

namespace OfferService.Commands;

public record UpdateOfferSeatsCommand(string id, int firstClassSeatsDiff, int secondClassSeatsDiff) : IRequest;

public class UpdateOfferSeatsCommandHandler : IRequestHandler<UpdateOfferSeatsCommand>
{
    private readonly OfferContext _context;

    private readonly IMediator _mediator;

    public UpdateOfferSeatsCommandHandler(OfferContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(UpdateOfferSeatsCommand request, CancellationToken cancellationToken)
    {
        var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == request.id);
        if (offer == null)
        {
            throw new Exception("Offer not found");
        }
        
        var @event = new OfferSeatsUpdatedEvent
        {
            Id = request.id,
            FirstClassSeatsDiff = request.firstClassSeatsDiff,
            SecondClassSeatsDiff = request.secondClassSeatsDiff
        };

        _context.DomainEvents.Add(@event);
        await _context.SaveChangesAsync();

        await _mediator.Publish(@event);
    }
}