using MassTransit;

namespace reservation_service.Events;

public class Sender
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public Sender(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Send<T>(T message, string queueName)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));
        await endpoint.Send(message);
        Console.WriteLine(" [x] Sent {0}", message.GetType().Name);
    }
}