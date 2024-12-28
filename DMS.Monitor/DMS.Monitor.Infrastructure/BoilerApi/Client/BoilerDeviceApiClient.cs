namespace DMS.Monitor.Infrastructure.BoilerApi.Client;

internal class BoilerDeviceApiClient : IBoilerDeviceApiClient
{
    private readonly HttpClient _httpClient;

    public BoilerDeviceApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<double> GetTemperatureAsync()
    {
        var response = await _httpClient.GetAsync("temperature");
        response.EnsureSuccessStatusCode();

        var temperatureFahrenheit = double.Parse(await response.Content.ReadAsStringAsync());
        return temperatureFahrenheit;
    }
}
