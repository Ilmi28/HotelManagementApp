using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.ReservationRepositories;

public class ReservationServiceRepository(AppDbContext context) : IReservationServiceRepository
{
    public async Task AddReservationService(ReservationService reservationService, CancellationToken ct)
    {
        context.Attach(reservationService.HotelService);
        context.Attach(reservationService.Reservation);
        await context.ReservationServices.AddAsync(reservationService, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveReservationService(ReservationService reservationService, CancellationToken ct)
    {
        context.ReservationServices.Remove(reservationService);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<ReservationService>> GetReservationServicesByReservationId(int reservationId, CancellationToken ct)
    {
        return await context.ReservationServices
                .AsNoTracking()
            .Include(x => x.Reservation)
            .Include(x => x.HotelService)
            .Where(x => x.Reservation.Id == reservationId).ToListAsync(ct);
    }
}