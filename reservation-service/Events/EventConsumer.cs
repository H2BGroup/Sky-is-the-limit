using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using reservation_service.Models;
using reservation_service.Services;

namespace reservation_service.Events;

public class EventConsumer
{
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;

    private readonly IServiceProvider _serviceProvider;

    public EventConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
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


        _channel.ExchangeDeclare(exchange: "BookingUnavailable", type: ExchangeType.Fanout, durable: true);
        _channel.QueueDeclare(queue: "BookingUnavailable_BookingSerivce", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: "BookingUnavailable_BookingSerivce", exchange: "BookingUnavailable", routingKey: "");
        var bookingUnavailableConsumer = new EventingBasicConsumer(_channel);
        bookingUnavailableConsumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received BookingUnavailable {0}", message);
            BookingUnavailable? bookingUnavailable = JsonSerializer.Deserialize<BookingUnavailable>(message);
            if (bookingUnavailable != null)
            {
                HandleBookingUnavailable(bookingUnavailable);
            }
        };
        _channel.BasicConsume(queue: "BookingUnavailable_BookingSerivce", autoAck: true, consumer: bookingUnavailableConsumer);


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
        using var scope = _serviceProvider.CreateScope();
        IBookingService _bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
        Booking? booking = _bookingService.GetBooking(bookingAvailable.Id);
        if (booking != null && booking.Status == BookingStatus.Pending)
        {
            booking.Status = BookingStatus.Reserved;
            _bookingService.Update(booking);
            Console.WriteLine(" [x] Updated Booking Status {0}", booking);
        }
        //TODO: Start timer for booking expiration
    }

    public void HandleBookingUnavailable(BookingUnavailable bookingUnavailable)
    {
        Console.WriteLine(" [x] Received BookingUnavailable {0}", bookingUnavailable);
        using var scope = _serviceProvider.CreateScope();
        IBookingService _bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
        _bookingService.Delete(bookingUnavailable.Id);
        Console.WriteLine(" [x] Deleted Booking {0}", bookingUnavailable.Id);
    }

    public void HandlePaymentSucceeded(PaymentSucceeded paymentSucceeded)
    {
        Console.WriteLine(" [x] Received PaymentSucceeded {0}", paymentSucceeded);
        using var scope = _serviceProvider.CreateScope();
        IBookingService _bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
        Booking? booking = _bookingService.GetBooking(paymentSucceeded.BookingId);
        if (booking != null && booking.Status == BookingStatus.Reserved)
        {
            booking.Status = BookingStatus.Confirmed;
            _bookingService.Update(booking);
            Console.WriteLine(" [x] Updated Booking Status {0}", booking);
        }
        //TODO: Send BookingConfirmed event (maybe not?)
    }
}