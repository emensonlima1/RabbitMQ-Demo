using Common.DTOs;
using Common.Interfaces;
using Infrastructure.MessageBroker;

namespace Domain.Events;

public class EventBus : IEventBus
{
    private static readonly RabbitMQProvider RabbitProvider = new();
    
    public void Publish(PublishEventDto eventDto)
    {
        RabbitProvider.Publish(eventDto);   
    }

    public void Subscribe(SubscribeEventDto subscribeEventDto, IEventHandler eventHandler)
    {
        RabbitProvider.Subscribe(subscribeEventDto, eventHandler);
    }
}