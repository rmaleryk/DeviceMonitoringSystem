namespace DMS.Monitor.Infrastructure.BoilerApi.Client;

public interface IBoilerApiClient
{
    Task<double> GetTemperatureFahrenheitAsync(Guid id, CancellationToken cancellationToken);
}
