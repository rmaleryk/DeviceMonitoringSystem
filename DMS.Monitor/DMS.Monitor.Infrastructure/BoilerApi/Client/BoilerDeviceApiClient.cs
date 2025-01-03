namespace DMS.Monitor.Infrastructure.BoilerApi.Client;

internal class BoilerDeviceApiClient(HttpClient httpClient) : IBoilerDeviceApiClient
{
    public async Task<double> GetTemperatureAsync()
    {
        var response = await httpClient.GetAsync("temperature");
        response.EnsureSuccessStatusCode();

        var temperatureFahrenheit = double.Parse(await response.Content.ReadAsStringAsync());
        return temperatureFahrenheit;
    }
}
