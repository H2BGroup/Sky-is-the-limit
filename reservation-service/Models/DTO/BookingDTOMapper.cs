namespace reservation_service.Models.DTO;

public static class BookingDTOMapper
{
    public static GetBookingResponse BookingToResponse(Booking reservation)
    {
        return new GetBookingResponse
        {
            Id = reservation.Id,
            User = new GetBookingResponse.SimpleUser
            {
                Id = reservation.User!.Id,
                Login = reservation.User.Login
            },
            Offer = new GetBookingResponse.SimpleOffer
            {
                Id = reservation.Offer!.Id,
                Origin = reservation.Offer.Origin,
                Destination = reservation.Offer.Destination,
                DepartureDate = reservation.Offer.DepartureDate
            },
            FirstClassSeats = reservation.FirstClassSeats,
            SecondClassSeats = reservation.SecondClassSeats,
            RegisteredBaggage = reservation.RegisteredBaggage,
            CarryOnBaggage = reservation.CarryOnBaggage,
            PriorityBoarding = reservation.PriorityBoarding,
            Insurance = reservation.Insurance,
            Price = reservation.Price,
            Status = reservation.Status
        };
    }

    public static GetBookingsResponse BookingsToResponse(IEnumerable<Booking> bookings)
    {
        return new GetBookingsResponse
        {
            Bookings = bookings.Select(r => new GetBookingsResponse.SimpleBooking
            {
                Id = r.Id
            })
        };
    }

    public static Booking RequestToBooking(string id, PutBookingRequest request)
    {
        return new Booking
        {
            Id = id,
            UserId = request.UserId ,
            OfferId = request.OfferId ,
            FirstClassSeats = request.FirstClassSeats,
            SecondClassSeats = request.SecondClassSeats,
            RegisteredBaggage = request.RegisteredBaggage,
            CarryOnBaggage = request.CarryOnBaggage,
            PriorityBoarding = request.PriorityBoarding,
            Insurance = request.Insurance,
            Price = request.Price,
            Status = BookingStatus.Pending
        };
    }
}
