using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Services;

public interface IPricingService
{
    Task<decimal> CalculatePriceForOrder(Order order, CancellationToken ct);
}