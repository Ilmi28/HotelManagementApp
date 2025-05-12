using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface IConfirmedOrderRepository
{
    Task AddConfirmedOrder(ConfirmedOrder order, CancellationToken ct);
    Task<ICollection<ConfirmedOrder>> GetConfirmedOrders(CancellationToken ct);
    Task<ICollection<ConfirmedOrder>> GetConfirmedOrdersByGuestId(string guestId, CancellationToken ct);
    Task<ConfirmedOrder?> GetConfirmedOrderByOrderId(int id, CancellationToken ct);
    Task DeleteConfirmedOrder(ConfirmedOrder order, CancellationToken ct);
}
