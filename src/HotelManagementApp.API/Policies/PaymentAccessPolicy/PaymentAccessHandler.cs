using System.Security.Claims;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.API.Policies.PaymentAccessPolicy;

public class PaymentAccessHandler(IPaymentRepository paymentRepository, IOrderRepository orderRepository) : AuthorizationHandler<PaymentAccessRequirement, int>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PaymentAccessRequirement requirement, int paymentId)
    {
        try
        {
            var payment = await paymentRepository.GetPaymentById(paymentId)
                          ?? throw new PaymentNotFoundException($"Payment with id {paymentId} not found");
            var order = await orderRepository.GetOrderById(payment.OrderId)
                        ?? throw new OrderNotFoundException($"Order with id {payment.OrderId} not found");
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            if (order.UserId == userId)
                context.Succeed(requirement);
            var userRoles = context.User.Claims.Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value).ToList();
            if (userRoles.Contains("Admin") || userRoles.Contains("Manager") || userRoles.Contains("Staff"))
                context.Succeed(requirement);
        }
        catch (Exception ex) { context.Fail(); }
    }
}