using MassTransit;

namespace reservation_service.Events;

public class Publisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public Publisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Publish<T>(T message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        await _publishEndpoint.Publish(message);
    }
}