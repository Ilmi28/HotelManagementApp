using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface IPendingOrderRepository
{
    Task AddPendingOrder(PendingOrder order, CancellationToken ct);
    Task<PendingOrder?> GetPendingOrderById(int id, CancellationToken ct);
    Task<ICollection<PendingOrder>> GetPendingOrders(CancellationToken ct);
    Task<PendingOrder?> GetPendingOrderByOrderId(int orderId, CancellationToken ct);
    Task DeletePendingOrder(PendingOrder pendingOrder, CancellationToken ct);
}
