using System.Security.Claims;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.API.Policies.PaymentAccessPolicy;

public class PaymentAccessHandler(IPaymentRepository paymentRepository) : AuthorizationHandler<PaymentAccessRequirement, int>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PaymentAccessRequirement requirement, int paymentId)
    {
        var payment = await paymentRepository.GetPaymentById(paymentId);
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (payment != null && payment.Order.UserId == userId)
            context.Succeed(requirement);
        var userRoles = context.User.Claims.Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value).ToList();
        if (userRoles.Contains("Admin") || userRoles.Contains("Manager") || userRoles.Contains("Staff"))
            context.Succeed(requirement);
    }
}