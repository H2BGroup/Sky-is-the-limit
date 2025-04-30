using OfferService.Models;
using System;
using Microsoft.EntityFrameworkCore;
using OfferService.Events;
using shared.Events;

namespace OfferService.Services
{
    public class OfferService
    {
        private readonly OfferContext _context;
        private readonly Publisher _publisher;

        public OfferService(OfferContext context, Publisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<IEnumerable<Offer>> GetAllOffers()
        {
            return await _context.Offers.ToListAsync();
        }

        public async Task<Offer?> GetOfferById(string id)
        {
            return await _context.Offers.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Offer> CreateOffer(Offer offer)
        {
            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();
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
            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
            await _publisher.Publish(new OfferUpdatedEvent
            {
                Id = offer.Id,
                Origin = offer.Origin,
                Destination = offer.Destination,
                DepartureDate = offer.DepartureDateTime
            });
            return offer;
        }

        public async Task<bool> DeleteOffer(string id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null) return false;

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

