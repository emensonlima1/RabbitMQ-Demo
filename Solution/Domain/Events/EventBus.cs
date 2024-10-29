using Common.DTOs;
using Common.Interfaces;
using Infrastructure.MessageBroker;

namespace Domain.Events;

public class EventBus : IEventBus
{
    private static readonly RabbitMQProvider RabbitProvider = new();
    
    public void Publish(EventDto eventDto)
    {
        RabbitProvider.Publish(eventDto);   
    }

    public void Subscribe(EventDto eventDto, IEventHandler eventHandler)
    {
        RabbitProvider.Subscribe(eventDto, eventHandler);
    }
}