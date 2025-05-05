using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface IOrderRepository
{
    Task AddOrder(Order order);
    Task<Order?> GetOrderById(int id);
    Task<ICollection<Order>> GetAllOrders();
    Task<ICollection<Order>> GetOrdersByGuestId(string guestId);
    Task UpdateOrder(Order order);
    Task DeleteOrder(Order order);
}
