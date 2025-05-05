using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;

public interface ICityRepository
{
    Task AddCity(City city, CancellationToken ct);
    Task<City?> GetCityById(int id, CancellationToken ct);
    Task<ICollection<City>> GetCitiesByCountry(string country, CancellationToken ct);
    Task<ICollection<City>> GetAllCities(CancellationToken ct);
    Task UpdateCity(City city, CancellationToken ct);
    Task DeleteCity(int id, CancellationToken ct);
    Task<ICollection<string>> GetCountries(CancellationToken ct);
}
