namespace Common.DTOs;

public class PublishEventDto(string exchange, string exchangeType, string routingKey, string message)
{
    public string Exchange { get; set;} = exchange;
    public string ExchangeType { get; set;} = exchangeType;
    public string RoutingKey { get; set;} = routingKey;
    public string Message { get; set;} = message;
}