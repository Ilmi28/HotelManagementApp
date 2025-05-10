using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;

public interface IReservationParkingRepository
{
    Task AddReservationParking(ReservationParking reservationParking, CancellationToken ct = default);
    Task RemoveReservationParking(ReservationParking reservationParking, CancellationToken ct = default);
    Task<ICollection<ReservationParking>> GetReservationParkingsByReservationId(int reservationId, CancellationToken ct = default);
}