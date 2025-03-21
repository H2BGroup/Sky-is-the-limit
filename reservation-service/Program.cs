using Microsoft.EntityFrameworkCore;
using reservation_service.Models;
using reservation_service.Services;
using reservation_service.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ReservationContext>(options =>
{
    options.UseInMemoryDatabase("Reservations");
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddSingleton<EventConsumer>();

var app = builder.Build();

// Seed the database with sample data TODO: delete later
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ReservationContext>();
    reservation_service.DataInitializer.Initialize(context);
}

var eventConsumer = app.Services.GetRequiredService<EventConsumer>();
eventConsumer.Consume();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
