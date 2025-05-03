using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OfferService.Models.DomainEvents;

public class DomainEvent : INotification
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}