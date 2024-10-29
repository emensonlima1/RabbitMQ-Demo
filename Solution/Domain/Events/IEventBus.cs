using Common.DTOs;
using Common.Interfaces;

namespace Domain.Events;

public interface IEventBus
{
    void Publish(EventDto eventDto);
    void Subscribe(EventDto eventDto, IEventHandler eventHandler);
}