using System.Security.Claims;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementApp.API.Policies.ReservationAccessPolicy;

public class ReservationAccessHandler(IReservationRepository reservationRepository) : AuthorizationHandler<ReservationAccessRequirement, int>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ReservationAccessRequirement requirement,
        int reservationId)
    {
        var reservation = await reservationRepository.GetReservationById(reservationId);
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        if (reservation != null && reservation.Order.UserId == userId)
            context.Succeed(requirement);
        else if (context.User.IsInRole("Admin") || context.User.IsInRole("Manager") || context.User.IsInRole("Staff"))
            context.Succeed(requirement);
    }
}