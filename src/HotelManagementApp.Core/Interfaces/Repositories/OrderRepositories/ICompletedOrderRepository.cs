using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface ICompletedOrderRepository
{
    Task AddCompletedOrder(CompletedOrder order, CancellationToken ct);
    Task<ICollection<CompletedOrder>> GetAllCompletedOrders(CancellationToken ct);
    Task<ICollection<CompletedOrder>> GetCompletedOrdersByGuestId(string guestId, CancellationToken ct);
    Task<CompletedOrder?> GetCompletedOrderByOrderId(int id, CancellationToken ct);
    Task DeleteCompletedOrder(CompletedOrder order, CancellationToken ct);
}
