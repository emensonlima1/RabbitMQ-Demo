namespace Common.DTOs;

public class ExchangeDto(string exchange, string exchangeType, string routingKey)
{
    public string Exchange { get; set; } = exchange;
    public string ExchangeType { get; set; } = exchangeType;
    public string RoutingKey { get; set; } = routingKey;
}