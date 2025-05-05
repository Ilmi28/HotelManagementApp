using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.HotelRepositories;

public class CityRepository(AppDbContext context) : ICityRepository
{
    public async Task AddCity(City city, CancellationToken ct)
    {
        await context.Cities.AddAsync(city, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<City?> GetCityById(int id, CancellationToken ct)
    {
        return await context.Cities
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<City>> GetAllCities(CancellationToken ct)
    {
        return await context.Cities.ToListAsync(ct);
    }

    public async Task UpdateCity(City city, CancellationToken ct)
    {
        context.Cities.Update(city);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteCity(int id, CancellationToken ct)
    {
        var city = await GetCityById(id, ct);
        if (city != null)
        {
            context.Cities.Remove(city);
            await context.SaveChangesAsync(ct);
        }
    }

    public async Task<ICollection<City>> GetCitiesByCountry(string country, CancellationToken ct)
    {
        return await context.Cities.Where(c => c.Country == country)
            .ToListAsync(ct);
    }

    public async Task<ICollection<string>> GetCountries(CancellationToken ct)
    {
        return await context.Cities
            .Select(c => c.Country)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(ct);
    }
}
