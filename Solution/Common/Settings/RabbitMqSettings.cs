namespace Common.Configurations;

public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public int Port { get; set; } = 5672;
    public long RequestedConnectionTimeout { get; set; } = 60000;
    public bool AutomaticRecoveryEnabled { get; set; } = true;
    public long NetworkRecoveryInterval { get; set; } = 60000;
}