using Domain.BusinessRules.Interfaces;

namespace Transaction.Worker;

public class Worker(ILogger<Worker> logger, ITransactionRule transactionRule) : BackgroundService
{
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            
            transactionRule.Register();

            await Task.Delay(1000, stoppingToken);
        }
    }
}