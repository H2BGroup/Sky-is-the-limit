using MassTransit;
using MassTransit.Serialization;
using notification_service.Events.Notifications;
using shared.Events;

namespace notification_service.Events.Consumers;

public class OfferUpdatedConsumer : IConsumer<OfferUpdatedEvent>
{
    private readonly INotificationSender _notificationSender;
    public OfferUpdatedConsumer(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task Consume(ConsumeContext<OfferUpdatedEvent> context)
    {
        var message = context.Message;
        Console.WriteLine(" [x] Received OfferUpdated {0}", message.Id);
        if (message != null)
        {
            await _notificationSender.NotifyOfferUpdated(message);
        }
    }
}