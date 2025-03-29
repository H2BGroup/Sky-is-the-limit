namespace reservation_service.Events;

public interface IEventProducer
{
    void Publish<T>(string exchange, T message);
}