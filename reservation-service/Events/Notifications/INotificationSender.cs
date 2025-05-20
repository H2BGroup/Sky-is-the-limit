using shared.Events;

namespace reservation_service.Events.Notifications;
public interface INotificationSender
{
    Task NotifyBookingConfirmed(BookingConfirmedEvent @event);
}