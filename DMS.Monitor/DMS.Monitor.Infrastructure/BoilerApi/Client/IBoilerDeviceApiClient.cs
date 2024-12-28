namespace DMS.Monitor.Infrastructure.BoilerApi.Client;

public interface IBoilerDeviceApiClient
{
    Task<double> GetTemperatureAsync();
}
