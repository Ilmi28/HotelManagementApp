using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Services;

public interface ICityService
{
    IAsyncEnumerable<City> FetchCities(CancellationToken ct);
}
