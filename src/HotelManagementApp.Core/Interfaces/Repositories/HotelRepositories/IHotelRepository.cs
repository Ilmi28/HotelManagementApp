using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;

public interface IHotelRepository 
{
    Task AddHotel(Hotel model, CancellationToken ct);
    Task RemoveHotel(Hotel hotel, CancellationToken ct);
    Task<ICollection<Hotel>> GetAllHotels(CancellationToken ct);
    Task<Hotel?> GetHotelById(int id, CancellationToken ct);
    Task UpdateHotel(Hotel hotel, CancellationToken ct);
}
