using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace reservation_service.Events;

public class EventProducer : IEventProducer
{
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;

    public EventProducer(IConfiguration configuration)
    {
        _configuration = configuration;
        ConnectionFactory factory = new();
        string? connectionString = _configuration.GetConnectionString("RabbitMQ");
        if (connectionString == null)
        {
            throw new ArgumentNullException("RabbitMQ connection string not found");
        }
        factory.Uri = new Uri(connectionString);
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Publish<T>(string exchange, T message)
    {
        Console.WriteLine(" [x] Sending {0} to {1}", message, exchange);
        _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, durable: true);
        string jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        _channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);
        Console.WriteLine(" [x] Sent {0}", jsonMessage);
    }
}