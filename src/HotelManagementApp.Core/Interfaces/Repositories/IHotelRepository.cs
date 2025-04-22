using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IHotelRepository 
{
    Task AddHotel(HotelModel model, CancellationToken ct);
    Task RemoveHotel(int id, CancellationToken ct);
    Task<ICollection<HotelModel>> GetAllHotels(CancellationToken ct);
    Task<HotelModel?> GetHotelById(int id, CancellationToken ct);
    Task UpdateHotel(HotelModel hotel, CancellationToken ct);
}
