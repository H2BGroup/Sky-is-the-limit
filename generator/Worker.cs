using Generator.Events;
using Generator.Services;
using Microsoft.Extensions.DependencyInjection;
using shared.Events;
using System.Text.Json;

namespace Generator;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Random _random = new();
    private readonly ILogger<Worker> _logger;
    private readonly IOfferService _offerService;

    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger, IOfferService offerService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _offerService = offerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delaySeconds = int.TryParse(Environment.GetEnvironmentVariable("GENERATOR_DELAY_SECONDS"), out var seconds) ? seconds : 5;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var offer = await _offerService.GetRandomOfferAsync();

                _logger.LogInformation("Generator get offer: {Offer}", JsonSerializer.Serialize(offer));

                var newPrice = offer.Price + (_random.NextDouble() * 0.6 - 0.3) * offer.Price;
                newPrice = Math.Round(newPrice, 2);

                _logger.LogInformation("Generator set new price: {NewPrice}", newPrice);

                var newFirstClassSeats = offer.SeatsFirstClass + _random.Next(offer.SeatsFirstClass * -1, offer.SeatsFirstClass);

                _logger.LogInformation("Generator set new first class seats: {NewFirstClassSeats}", newFirstClassSeats);

                var newSecondClassSeats = offer.SeatsEconomy + _random.Next(offer.SeatsEconomy * -1, offer.SeatsEconomy);

                _logger.LogInformation("Generator set new second class seats: {NewSecondClassSeats}", newSecondClassSeats);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var publisher = scope.ServiceProvider.GetRequiredService<Publisher>();

                    await publisher.Publish(new GeneratorOfferUpdatedEvent
                    {
                        Id = offer.Id,
                        Price = newPrice,
                        FirstClassSeats = newFirstClassSeats,
                        SecondClassSeats = newSecondClassSeats
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching a random offer.");
            }

            Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken).Wait(stoppingToken);
        }
    }
}
