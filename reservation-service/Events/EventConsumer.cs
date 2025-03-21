using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace reservation_service.Events;

public class EventConsumer
{
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;

    public EventConsumer(IConfiguration configuration)
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

    public void Consume()
    {
        _channel.ExchangeDeclare(exchange: "BookingAvailable", type: ExchangeType.Fanout, durable: true);
        _channel.QueueDeclare(queue: "BookingAvailable_BookingSerivce", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: "BookingAvailable_BookingSerivce", exchange: "BookingAvailable", routingKey: "");

        var bookingAvailableConsumer = new EventingBasicConsumer(_channel);
        bookingAvailableConsumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received BookingAvailable {0}", message);
            BookingAvailable? bookingAvailable = JsonSerializer.Deserialize<BookingAvailable>(message);
            if (bookingAvailable != null)
            {
                HandleBookingAvailable(bookingAvailable);
            }
        };
        _channel.BasicConsume(queue: "BookingAvailable_BookingSerivce", autoAck: true, consumer: bookingAvailableConsumer);


        _channel.ExchangeDeclare(exchange: "PaymentSucceeded", type: ExchangeType.Fanout, durable: true);
        _channel.QueueDeclare(queue: "PaymentSucceeded_BookingSerivce", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: "PaymentSucceeded_BookingSerivce", exchange: "PaymentSucceeded", routingKey: "");

        var paymentSucceededConsumer = new EventingBasicConsumer(_channel);
        paymentSucceededConsumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received PaymentSucceeded {0}", message);
            PaymentSucceeded? paymentSucceeded = JsonSerializer.Deserialize<PaymentSucceeded>(message);
            if (paymentSucceeded != null)
            {
                HandlePaymentSucceeded(paymentSucceeded);
            }
        };
        _channel.BasicConsume(queue: "PaymentSucceeded_BookingSerivce", autoAck: true, consumer: paymentSucceededConsumer);
    }

    public void HandleBookingAvailable(BookingAvailable bookingAvailable)
    {
        Console.WriteLine(" [x] Received BookingAvailable {0}", bookingAvailable);
    }

    public void HandlePaymentSucceeded(PaymentSucceeded paymentSucceeded)
    {
        Console.WriteLine(" [x] Received PaymentSucceeded {0}", paymentSucceeded);
    }
}