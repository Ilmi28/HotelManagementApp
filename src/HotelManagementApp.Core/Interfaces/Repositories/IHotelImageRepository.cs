using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IHotelImageRepository
{
    Task AddHotelImage(HotelImage hotelImage, CancellationToken ct);
    Task<ICollection<HotelImage>> GetHotelImagesByHotelId(int hotelId, CancellationToken ct);
    Task RemoveHotelImagesByHotelId(int hotelId, CancellationToken ct);
}
