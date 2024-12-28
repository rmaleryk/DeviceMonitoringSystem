namespace DMS.ClientApp.Configuration;

public sealed record class RabbitMqSettings
{
    public string Host { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
