using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.ReservationRepositories;

public class ReservationRepository(AppDbContext context) : IReservationRepository
{
    public async Task AddReservation(Reservation reservation, CancellationToken ct = default)
    {
        context.Attach(reservation.Room);
        context.Attach(reservation.Order);
        await context.Reservations.AddAsync(reservation, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Reservation?> GetReservationById(int id, CancellationToken ct = default)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.ReservationParkings)
            .Include(x => x.ReservationServices)
            .Include(x => x.Order)
            .Include(x => x.Room)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
    }

    public async Task<ICollection<Reservation>> GetAllReservations(CancellationToken ct = default)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.ReservationParkings)
            .Include(x => x.ReservationServices)
            .Include(x => x.Order)
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<ICollection<Reservation>> GetReservationsByRoomId(int roomId, CancellationToken ct = default)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.ReservationParkings)
            .Include(x => x.ReservationServices)
            .Include(x => x.Order)
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .Where(x => x.Room.Id == roomId)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<ICollection<Reservation>> GetReservationsByHotelId(int hotelId, CancellationToken ct = default)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.ReservationParkings)
            .Include(x => x.ReservationServices)
            .Include(x => x.Order)
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
            .Include(x => x.ReservationParkings)
            .Include(x => x.ReservationServices)
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .Include(x => x.Order)
            .Where(x => x.Order.UserId == guestId)
            .ToListAsync(cancellationToken: ct);
}

    public async Task<ICollection<Reservation>> GetReservationsByOrderId(int orderId, CancellationToken ct)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.ReservationParkings)
            .Include(x => x.ReservationServices)
            .Include(x => x.Order)
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .Where(x => x.Order.Id == orderId)
            .ToListAsync(cancellationToken: ct);
    }
}