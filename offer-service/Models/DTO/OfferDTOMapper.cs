namespace OfferService.Models.DTO
{
    public static class OfferDTOMapper
    {
        public static GetOfferResponse OfferToResponse(Offer offer)
        {
            return new GetOfferResponse
            {
                Id = offer.Id,
                Departure = offer.Origin,
                Arrival = offer.Destination,
                Price = offer.Price,
                Datetime = offer.DepartureDateTime.ToString("yyyy-MM-ddTHH:mm"),
                Duration = $"{offer.FlightLength.Hours}h {offer.FlightLength.Minutes}m",
                Airline = offer.Airline,
                SeatsFirstClass = offer.FirstClassSeats,
                SeatsEconomy = offer.SecondClassSeats
            };
        }

        public static GetOffersResponse OffersToResponse(IEnumerable<Offer> offers)
        {
            return new GetOffersResponse
            {
                Offers = offers.Select(o => new GetOffersResponse.SimpleOffer
                {
                    Id = o.Id,
                    Departure = o.Origin,
                    Arrival = o.Destination,
                    Price = o.Price,
                    Datetime = o.DepartureDateTime.ToString("yyyy-MM-ddTHH:mm"),
                    Duration = $"{o.FlightLength.Hours}h {o.FlightLength.Minutes}m",
                    Airline = o.Airline
                })
            };
        }

        public static Offer RequestToOffer(string id, PutOfferRequest request)
        {
            var timeParts = request.Duration.Split("h");
            var hours = int.Parse(timeParts[0].Trim());
            var minutes = int.Parse(timeParts[1].Replace("m", "").Trim());

            return new Offer
            {
                Id = id,
                Origin = request.Departure,
                Destination = request.Arrival,
                DepartureDateTime = DateTime.Parse(request.Datetime),
                FlightLength = new TimeSpan(hours, minutes, 0),
                Airline = request.Airline,
                FirstClassSeats = request.SeatsFirstClass,
                SecondClassSeats = request.SeatsEconomy,
                Price = request.Price
            };
        }
    }
}
