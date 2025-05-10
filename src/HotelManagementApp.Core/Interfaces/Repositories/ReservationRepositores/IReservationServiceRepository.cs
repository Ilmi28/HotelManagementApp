using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;

public interface IReservationServiceRepository
{
    Task AddReservationService(ReservationService reservationService, CancellationToken ct);
    Task RemoveReservationService(ReservationService reservationService, CancellationToken ct);
    Task<ICollection<ReservationService>> GetReservationServicesByReservationId(int reservationId, CancellationToken ct);
}