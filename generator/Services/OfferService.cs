using Generator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Services
{
    public class OfferService : IOfferService
    {
        private readonly HttpClient _httpClient;
        private readonly Random _random = new();

        public OfferService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Offer> GetRandomOfferAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<OffersResponse>("offer");

            if (response?.Offers == null || response.Offers.Count == 0)
                throw new InvalidOperationException("No offers returned from API.");

            var randomOffer = response.Offers[_random.Next(response.Offers.Count)];

            return randomOffer;
        }
    }
}
