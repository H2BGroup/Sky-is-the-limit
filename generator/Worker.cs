using Generator.Services;

namespace Generator;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOfferService _offerService;

    public Worker(ILogger<Worker> logger, IOfferService offerService)
    {
        _logger = logger;
        _offerService = offerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var offer = await _offerService.GetRandomOfferAsync();

                _logger.LogInformation("Generator get offer: {OfferId}", offer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching a random offer.");
            }

            Task.Delay(TimeSpan.FromSeconds(10), stoppingToken).Wait(stoppingToken);
        }
    }
}
