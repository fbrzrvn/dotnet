namespace FreeStuff.Shared.Infrastructure.MessageBroker;

public sealed class RabbitMQConfig
{
    public string Host     { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
