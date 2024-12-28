namespace DMS.Monitor.Infrastructure.BoilerApi.Configuration;

public class DevicesSettings
{
    public BoilerDeviceApiSettings? BoilerDeviceApi { get; set; }

    public class BoilerDeviceApiSettings
    {
        public string Host { get; set; } = string.Empty;
    }
}
