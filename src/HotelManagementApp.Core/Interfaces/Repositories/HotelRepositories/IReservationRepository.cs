using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;

public interface IReservationRepository
{
    Task AddReservation(Reservation reservation, CancellationToken ct);
    Task<Reservation?> GetReservationById(int id, CancellationToken ct);
    Task<ICollection<Reservation>> GetAllReservations(CancellationToken ct);
    Task<ICollection<HotelRoom>> GetReservationsByRoomId(int roomId, CancellationToken ct);
    Task<ICollection<Reservation>> GetReservationsByHotelId(int hotelId, CancellationToken ct);
    Task UpdateReservation(Reservation reservation, CancellationToken ct);
    Task DeleteReservation(Reservation reservation, CancellationToken ct);
    Task<ICollection<Reservation>> GetReservationsByGuestId(string guestId, CancellationToken ct);
    Task<ICollection<Reservation>> GetReservationsByOrderId(int orderId, CancellationToken ct);
}
