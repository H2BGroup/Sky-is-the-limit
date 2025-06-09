using OfferService.Models;
using OfferService.Events;
using shared.Events;
using MediatR;
using OfferService.Queries;
using OfferService.Commands;

namespace OfferService.Services
{
    public class OfferService
    {
        private readonly IMediator _mediator;
        private readonly Publisher _publisher;

        public OfferService(IMediator mediator, Publisher publisher)
        {
            _mediator = mediator;
            _publisher = publisher;
        }

        public async Task<IEnumerable<Offer>> GetAllOffers()
        {
            return await _mediator.Send(new GetOffersQuerry());
        }

        public async Task<Offer?> GetOfferById(string id)
        {
            return await _mediator.Send(new GetOfferByIdQuery(id));
        }

        public async Task<Offer> CreateOffer(Offer offer)
        {
            await _mediator.Send(new CreateOfferCommand(offer));
            await _publisher.Publish(new OfferCreatedEvent
            {
                Id = offer.Id,
                Origin = offer.Origin,
                Destination = offer.Destination,
                DepartureDate = offer.DepartureDateTime
            });
            return offer;
        }
        public async Task<Offer> UpdateOffer(Offer offer)
        {
            await _mediator.Send(new UpdateOfferCommand(offer));
            await _publisher.Publish(new OfferUpdatedEvent
            {
                Id = offer.Id,
                Origin = offer.Origin,
                Destination = offer.Destination,
                DepartureDate = offer.DepartureDateTime,
                FlightLength = offer.FlightLength,
                Airline = offer.Airline,
                FirstClassSeats = offer.FirstClassSeats,
                SecondClassSeats = offer.SecondClassSeats,
                Price = offer.Price
            });
            return offer;
        }

        public async Task<bool> UpdateOfferSeats(string id, int firstClassSeatsDiff, int secondClassSeatsDiff)
        {
            await _mediator.Send(new UpdateOfferSeatsCommand(id, firstClassSeatsDiff, secondClassSeatsDiff));
            return true;
        }

        public async Task<bool> DeleteOffer(string id)
        {
            return await _mediator.Send(new DeleteOfferCommand(id));
        }
    }
}

