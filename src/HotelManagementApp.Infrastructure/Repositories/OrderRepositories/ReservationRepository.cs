using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class ReservationRepository(AppDbContext context) : IReservationRepository
{
    public async Task AddReservation(Reservation reservation, CancellationToken ct = default)
    {
        await context.Reservations.AddAsync(reservation, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Reservation?> GetReservationById(int id, CancellationToken ct = default)
    {
        return await context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
    }

    public async Task<ICollection<Reservation>> GetAllReservations(CancellationToken ct = default)
    {
        return await context.Reservations
            .AsNoTracking()
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<ICollection<HotelRoom>> GetReservationsByRoomId(int roomId, CancellationToken ct = default)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .Where(x => x.Room.Id == roomId)
            .Select(x => x.Room)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<ICollection<Reservation>> GetReservationsByHotelId(int hotelId, CancellationToken ct = default)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .Where(x => x.Room.Hotel.Id == hotelId)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task UpdateReservation(Reservation reservation, CancellationToken ct = default)
    {
        context.Reservations.Update(reservation);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteReservation(Reservation reservation, CancellationToken ct = default)
    {
        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<Reservation>> GetReservationsByGuestId(string guestId, CancellationToken ct)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.Order.UserId == guestId)
            .ToListAsync(cancellationToken: ct);
}

    public async Task<ICollection<Reservation>> GetReservationsByOrderId(int orderId, CancellationToken ct)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.Order.Id == orderId)
            .ToListAsync(cancellationToken: ct);
    }
}