using Common.DTOs;
using Domain.BusinessRules.Interfaces;
using Domain.Events;

namespace Domain.BusinessRules;

public class TransactionRule(IEventBus eventBus) : ITransactionRule
{
    public void Register()
    {
        var data = new
        {
            Id = Guid.NewGuid(),
            Value = 10.00
        };

        var teste = new PublishEventDto("webhook.events",
            "direct",
            "webhook.created",
            data.ToString());
        
        eventBus.Publish(new PublishEventDto("webhook.events",
            "direct",
            "webhook.created",
            data.ToString()));
    }
}