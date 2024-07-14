namespace BoilerMonitor.Infrastructure.BoilerApi.Client
{
    public interface IBoilerApiClient
    {
        Task<double> GetTemperatureAsync();
    }
}
