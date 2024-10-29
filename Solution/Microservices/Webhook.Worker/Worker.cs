using Common.DTOs;
using Common.Interfaces;
using Domain.Events;

namespace Webhook.Worker;

public class Worker(ILogger<Worker> logger, IEventBus eventBus, IEventHandler eventHandler) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        }

        eventBus.Subscribe(
            new SubscribeEventDto
            (
                "webhook.created",
                "webhook.events",
                "direct",
                "webhook.created"
            ),
            eventHandler);
        return Task.CompletedTask;
    }
}