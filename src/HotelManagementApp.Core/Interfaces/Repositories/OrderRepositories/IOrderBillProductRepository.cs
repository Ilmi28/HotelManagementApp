using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface IOrderBillProductRepository
{
    Task AddOrderBillProduct(OrderBillProduct orderBillProduct, CancellationToken ct);
    Task DeleteOrderBillProduct(OrderBillProduct orderBillProduct, CancellationToken ct);
    Task<ICollection<OrderBillProduct>> GetOrderBillProductsByOrderId(int orderId, CancellationToken ct);
}