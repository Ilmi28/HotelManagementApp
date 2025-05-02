using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IHotelServiceRepository
{
    Task AddHotelService(HotelService hotelService, CancellationToken cancellationToken);
    Task<ICollection<HotelService>> GetHotelServicesByHotel(int hotelId, CancellationToken cancellationToken);
    Task<HotelService?> GetHotelServiceById(int id, CancellationToken cancellationToken);
    Task DeleteHotelService(HotelService hotelService, CancellationToken cancellationToken);
    Task UpdateHotelService(HotelService hotelService, CancellationToken cancellationToken);
}
