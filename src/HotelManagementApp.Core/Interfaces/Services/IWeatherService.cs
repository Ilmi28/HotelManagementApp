namespace HotelManagementApp.Core.Interfaces.Services;

public interface IWeatherService
{
    Task<double> GetTemperatureAsync(double latitude, double longitude);
}