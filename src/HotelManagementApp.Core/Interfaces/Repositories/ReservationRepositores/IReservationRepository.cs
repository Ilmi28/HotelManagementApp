using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;

public interface IReservationRepository
{
    Task AddReservation(Reservation reservation, CancellationToken ct = default);
    Task<Reservation?> GetReservationById(int id, CancellationToken ct = default);
    Task<ICollection<Reservation>> GetAllReservations(CancellationToken ct = default);
    Task<ICollection<Reservation>> GetReservationsByRoomId(int roomId, CancellationToken ct = default);
    Task<ICollection<Reservation>> GetReservationsByHotelId(int hotelId, CancellationToken ct = default);
    Task UpdateReservation(Reservation reservation, CancellationToken ct = default);
    Task DeleteReservation(Reservation reservation, CancellationToken ct = default);
    Task<ICollection<Reservation>> GetReservationsByGuestId(string guestId, CancellationToken ct = default);
    Task<ICollection<Reservation>> GetReservationsByOrderId(int orderId, CancellationToken ct = default);
}
