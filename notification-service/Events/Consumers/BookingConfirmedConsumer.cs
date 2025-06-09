using MassTransit;
using MassTransit.Serialization;
using notification_service.Events.Notifications;
using shared.Events;

namespace notification_service.Events.Consumers;

public class BookingConfirmedConsumer : IConsumer<BookingConfirmedEvent>
{
    private readonly INotificationSender _notificationSender;
    public BookingConfirmedConsumer(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task Consume(ConsumeContext<BookingConfirmedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine(" [x] Received BookingConfirmed {0}", message.Id);
        if (message != null)
        {
            await _notificationSender.NotifyBookingConfirmed(message);
        }
    }
}