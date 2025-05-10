using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.ReservationRepositories;

public class ReservationParkingRepository(AppDbContext context) : IReservationParkingRepository
{
    public async Task AddReservationParking(ReservationParking reservationParking, CancellationToken ct)
    {
        context.Attach(reservationParking.HotelParking);
        context.Attach(reservationParking.Reservation);
        await context.ReservationParkings.AddAsync(reservationParking, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveReservationParking(ReservationParking reservationParking, CancellationToken ct)
    {
        context.ReservationParkings.Remove(reservationParking);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<ReservationParking>> GetReservationParkingsByReservationId(int reservationId, CancellationToken ct)
    {
        return await context.ReservationParkings
                .AsNoTracking()
            .Include(x => x.Reservation)
            .Include(x => x.HotelParking)
            .Where(x => x.Reservation.Id == reservationId).ToListAsync(ct);
    }
}