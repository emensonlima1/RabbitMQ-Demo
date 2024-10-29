using Common.DTOs;
using Common.Interfaces;

namespace Domain.Events;

public interface IEventBus
{
    void Publish(PublishEventDto eventDto);
    void Subscribe(SubscribeEventDto subscribeEventDto, IEventHandler eventHandler);
}