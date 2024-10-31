using System.Collections;
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

        var exchanges = new List<ExchangeDto>();
        exchanges.Add(new ExchangeDto("webhook.events", "direct", "webhook.created"));

        eventBus.Subscribe(
            new SubscribeEventDto
            (
                "webhook.created",
                exchanges
            ),
            eventHandler);
        return Task.CompletedTask;
    }
}