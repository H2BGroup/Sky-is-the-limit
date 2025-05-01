using Microsoft.EntityFrameworkCore;
using reservation_service.Models;
using reservation_service.Services;
using MassTransit;
using reservation_service.Events.Consumers;
using reservation_service.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ReservationContext>(options =>
{
    // options.UseInMemoryDatabase("Reservations");
    options.UseMySQL(builder.Configuration.GetConnectionString("MySql")!);
});
builder.Services.AddMassTransit(config => {
    config.AddConsumer<BookingAvailableConsumer>();
    config.AddConsumer<BookingUnavailableConsumer>();
    config.AddConsumer<PaymentSucceededConsumer>();
    config.AddConsumer<OfferCreatedConsumer>();
    config.AddConsumer<OfferUpdatedConsumer>();

    config.SetEndpointNameFormatter(new DefaultEndpointNameFormatter("reservation-service", false));

    config.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ")!);
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddTransient<Publisher>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddHostedService<BookingExpirationWorker>();
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.SetIsOriginAllowed(_ => true);
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

var app = builder.Build();

// Seed the database with sample data TODO: delete later
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ReservationContext>();
    reservation_service.DataInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
