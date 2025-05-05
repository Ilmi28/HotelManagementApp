using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;

public interface IHotelParkingRepository
{
    Task AddHotelParking(HotelParking hotelParking, CancellationToken ct);
    Task DeleteHotelParking(HotelParking hotelParking, CancellationToken ct);
    Task<HotelParking?> GetHotelParkingById(int id, CancellationToken ct);
    Task<ICollection<HotelParking>> GetHotelParkingsByHotelId(int hotelId, CancellationToken ct);
    Task UpdateHotelParking(HotelParking hotelParking, CancellationToken ct);
}
