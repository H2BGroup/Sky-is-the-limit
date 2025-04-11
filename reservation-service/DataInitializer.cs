using Microsoft.EntityFrameworkCore;
using reservation_service.Models;
using System;
using System.Linq;

namespace reservation_service
{
    public static class DataInitializer
    {
        public static void Initialize(ReservationContext context)
        {
            context.Database.EnsureCreated();

            // Look for any existing data.
            if (context.Users.Any() && context.Offers.Any() && context.Bookings.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User { Id="1", Name = "Alice", Surname = "Smith", Login = "alice", Password = "password"},
                new User { Id="2", Name = "Bob", Surname = "Brown", Login = "bob", Password = "password"}
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

            var offers = new Offer[]
            {
                new Offer { Id="1", Origin = "Paris", Destination = "London", DepartureDate = DateTime.Now },
                new Offer { Id="2", Origin = "London", Destination = "Paris", DepartureDate = DateTime.Now },
                new Offer { Id="3", Origin = "Paris", Destination = "Berlin", DepartureDate = DateTime.Now }
            };

            foreach (var offer in offers)
            {
                context.Offers.Add(offer);
            }
            context.SaveChanges();

            var bookings = new Booking[]
            {
                new Booking { Id="1", UserId = users[0].Id, OfferId = offers[0].Id, FirstClassSeats = 1, SecondClassSeats = 2, RegisteredBaggage = 3, Price = 100, Status = BookingStatus.Confirmed},
            };

            foreach (var booking in bookings)
            {
                context.Bookings.Add(booking);
            }
            context.SaveChanges();
        }
    }
}