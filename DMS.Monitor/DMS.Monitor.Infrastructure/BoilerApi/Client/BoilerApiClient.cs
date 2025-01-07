namespace DMS.Monitor.Infrastructure.BoilerApi.Client;

internal class BoilerApiClient(HttpClient httpClient) : IBoilerApiClient
{
    public async Task<double> GetTemperatureFahrenheitAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{id}/temperature", cancellationToken);
        response.EnsureSuccessStatusCode();

        var temperatureFahrenheit = double.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
        return temperatureFahrenheit;
    }
}
