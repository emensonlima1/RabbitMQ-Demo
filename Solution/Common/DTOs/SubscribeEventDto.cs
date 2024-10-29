namespace Common.DTOs;

public class SubscribeEventDto(string queueName, string exchange, string exchangeType, string routingKey)
{
    public string QueueName { get; set; } = queueName;
    public string Exchange { get; set;} = exchange;
    public string ExchangeType { get; set;} = exchangeType;
    public string RoutingKey { get; set;} = routingKey;
}