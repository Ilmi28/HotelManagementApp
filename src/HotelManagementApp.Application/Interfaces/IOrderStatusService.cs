using HotelManagementApp.Application.Dtos;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Application.Interfaces;

public interface IOrderStatusService
{
    Task<OrderStatusesDto> GetOrderStatusesAsync(Order order, CancellationToken cancellationToken);
}