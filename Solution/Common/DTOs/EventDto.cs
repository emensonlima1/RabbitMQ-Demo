namespace Common.DTOs;

public class EventDto(string queueName, string exchange, string exchangeType, string routingKey, string message)
{
    public string QueueName { get; } = queueName;
    public string Exchange { get; } = exchange;
    public string ExchangeType { get; } = exchangeType;
    public string RoutingKey { get; } = routingKey;
    public string Message { get; } = message;
}