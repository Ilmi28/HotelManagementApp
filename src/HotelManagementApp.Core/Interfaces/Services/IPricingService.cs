using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Services;

public interface IPricingService
{
    Task<decimal> CalculatePriceForOrder(Order order, CancellationToken ct);
    Task<decimal> CalculatePriceForParking(HotelParking hotelParking, CancellationToken ct);
    Task<decimal> CalculatePriceForRoom(HotelRoom hotelRoom, CancellationToken ct);
    Task<decimal> CalculatePriceForService(HotelService hotelService, CancellationToken ct);
}