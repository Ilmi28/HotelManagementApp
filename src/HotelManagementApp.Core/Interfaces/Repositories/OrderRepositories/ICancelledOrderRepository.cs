using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface ICancelledOrderRepository
{
    Task AddCancelledOrder(CancelledOrder order, CancellationToken ct);
    Task<ICollection<CancelledOrder>> GetCancelledOrdersByGuestId(string guestId, CancellationToken ct);
    Task<ICollection<CancelledOrder>> GetCancelledOrders(CancellationToken ct);
    Task<CancelledOrder?> GetCancelledOrderById(int id, CancellationToken ct);
    Task DeleteCancelledOrder(CancelledOrder order, CancellationToken ct);
}
