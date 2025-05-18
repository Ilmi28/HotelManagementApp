using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Interfaces;

public interface IBillProductService
{
    Task AddBillProductsForOrder(Order order, CancellationToken ct);
}