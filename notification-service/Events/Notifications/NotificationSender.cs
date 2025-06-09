using Microsoft.AspNetCore.SignalR;
using shared.Events;

namespace notification_service.Events.Notifications;

public class NotificationSender : INotificationSender
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationSender(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyBookingConfirmed(BookingConfirmedEvent @event)
    {
        await _hubContext.Clients.All.SendAsync("BookingConfirmed", @event);
    }

    public async Task NotifyOfferUpdated(OfferUpdatedEvent @event)
    {
        await _hubContext.Clients.All.SendAsync("OfferUpdated", @event);
    }
}