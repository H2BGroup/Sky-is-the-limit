using MediatR;
using Microsoft.EntityFrameworkCore;
using OfferService.Models;
using OfferService.Models.DomainEvents;

namespace OfferService.Commands;

public record UpdateOfferCommand(Offer offer) : IRequest;

public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
{
    private readonly OfferContext _context;
    private readonly IMediator _mediator;

    public UpdateOfferCommandHandler(OfferContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = await _context.Offers.FirstOrDefaultAsync(x => x.Id == request.offer.Id);
        if (offer == null)
        {
            throw new Exception("Offer not found");
        }

        var @event = new OfferUpdatedEvent
        {
            Id = request.offer.Id,
            Origin = request.offer.Origin,
            Destination = request.offer.Destination,
            DepartureDateTime = request.offer.DepartureDateTime,
            FlightLength = request.offer.FlightLength,
            Airline = request.offer.Airline,
            FirstClassSeats = request.offer.FirstClassSeats,
            SecondClassSeats = request.offer.SecondClassSeats,
            Price = request.offer.Price
        };
        
        _context.DomainEvents.Add(@event);
        await _context.SaveChangesAsync();

        await _mediator.Publish(@event);
    }
}