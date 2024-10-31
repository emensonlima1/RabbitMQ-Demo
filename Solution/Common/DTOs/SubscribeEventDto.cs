namespace Common.DTOs;

public class SubscribeEventDto(
    string queueName,
    IList<ExchangeDto> exchanges)
{
    public string QueueName { get; } = queueName;
    public IList<ExchangeDto> Exchanges { get; set; } = exchanges;
}