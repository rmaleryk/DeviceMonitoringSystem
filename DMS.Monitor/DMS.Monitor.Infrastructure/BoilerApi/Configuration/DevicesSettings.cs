namespace DMS.Monitor.Infrastructure.BoilerApi.Configuration;

public class DevicesSettings
{
    public BoilerApiSettings? BoilerApi { get; set; }

    public class BoilerApiSettings
    {
        public string Host { get; set; } = string.Empty;
    }
}
