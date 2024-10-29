using Common.Interfaces;

namespace Webhook.Worker;

public class Handler : IEventHandler
{
    public Task Handle(string message)
    {
        return Task.CompletedTask;
    }
}