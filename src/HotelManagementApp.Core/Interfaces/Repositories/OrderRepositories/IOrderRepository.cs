using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface IOrderRepository
{
    Task AddOrder(Order order, CancellationToken ct = default);
    Task<Order?> GetOrderById(int id, CancellationToken ct = default);
    Task<ICollection<Order>> GetAllOrders( CancellationToken ct = default);
    Task<ICollection<Order>> GetOrdersByGuestId(string guestId, CancellationToken ct = default);
    Task UpdateOrder(Order order, CancellationToken ct = default);
    Task DeleteOrder(Order order, CancellationToken ct = default);
}
