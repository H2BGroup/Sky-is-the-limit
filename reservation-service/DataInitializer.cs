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
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User { Id="1", Name = "Alice", Surname = "Smith", Login = "alice", Password = "password"},
                new User { Id="2", Name = "Bob", Surname = "Brown", Login = "bob", Password = "password"},
                new User { Id="3", Name = "Charlie", Surname = "Davis", Login = "charlie", Password = "password"},
                new User { Id="4", Name = "David", Surname = "Evans", Login = "david", Password = "password"}
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
        }
    }
}