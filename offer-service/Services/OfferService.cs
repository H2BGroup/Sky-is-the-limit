using OfferService.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace OfferService.Services
{
    public class OfferService
    {
        private readonly OfferContext _context;

        public OfferService(OfferContext context)
        {
            _context = context;
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

