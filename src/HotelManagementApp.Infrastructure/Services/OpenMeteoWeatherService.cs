using System.Globalization;
using HotelManagementApp.Core.Interfaces.Services;
using System.Text.Json;

namespace HotelManagementApp.Infrastructure.Services;

public class OpenMeteoWeatherService(HttpClient httpClient) : IWeatherService
{
    private const string OpenMeteoBaseUrl = "https://api.open-meteo.com/v1/forecast";

    public async Task<double> GetTemperatureAsync(double latitude, double longitude)
    {
        var latitudeString = latitude.ToString(CultureInfo.InvariantCulture);
        var longitudeString = longitude.ToString(CultureInfo.InvariantCulture);
        var response = await httpClient.GetAsync($"{OpenMeteoBaseUrl}?latitude={latitudeString}&longitude={longitudeString}&current=temperature_2m");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var temp = doc.RootElement.GetProperty("current").GetProperty("temperature_2m").GetDouble();
        
        return temp;
    }
}