namespace reservation_service.Models.DTO;

public static class ReservationDTOMapper
{
    public static GetReservationResponse ReservationToResponse(Reservation reservation)
    {
        return new GetReservationResponse
        {
            Id = reservation.Id,
            User = new GetReservationResponse.SimpleUser
            {
                Id = reservation.User!.Id,
                Login = reservation.User.Login
            },
            Offer = new GetReservationResponse.SimpleOffer
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

    public static GetReservationsResponse ReservationsToResponse(IEnumerable<Reservation> reservations)
    {
        return new GetReservationsResponse
        {
            Reservations = reservations.Select(r => new GetReservationsResponse.SimpleReservation
            {
                Id = r.Id
            })
        };
    }

    public static Reservation RequestToReservation(string id, PutReservationRequest request)
    {
        return new Reservation
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
            Status = ReservationStatus.Pending
        };
    }
}
