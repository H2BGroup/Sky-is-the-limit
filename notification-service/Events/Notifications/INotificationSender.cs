using shared.Events;

namespace notification_service.Events.Notifications;
public interface INotificationSender
{
    Task NotifyBookingConfirmed(BookingConfirmedEvent @event);
}