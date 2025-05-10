using System.Security.Claims;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.API.Policies.OrderAccessPolicy;

public class OrderAccessHandler(IOrderRepository orderRepository) : AuthorizationHandler<OrderAccessRequirement, int>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderAccessRequirement requirement, int orderId)
    {
        var order = await orderRepository.GetOrderById(orderId);
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (order != null && order.UserId == userId)
            context.Succeed(requirement);
        var userRoles = context.User.Claims.Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value).ToList();
        if (userRoles.Contains("Admin") || userRoles.Contains("Manager") || userRoles.Contains("Staff"))
            context.Succeed(requirement);
    }
}