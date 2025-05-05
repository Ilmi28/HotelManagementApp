using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class ReservationRepository(AppDbContext context) : IReservationRepository
{
    public async Task AddReservation(Reservation reservation)
    {
        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();
    }

    public async Task<Reservation?> GetReservationById(int id)
    {
        return await context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Reservation>> GetAllReservations()
    {
        return await context.Reservations
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<HotelRoom>> GetReservationsByRoomId(int roomId)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .Where(x => x.Room.Id == roomId)
            .Select(x => x.Room)
            .ToListAsync();
    }

    public async Task<ICollection<Reservation>> GetReservationsByHotelId(int hotelId)
    {
        return await context.Reservations
            .AsNoTracking()
            .Include(x => x.Room)
            .ThenInclude(x => x.Hotel)
            .Where(x => x.Room.Hotel.Id == hotelId)
            .ToListAsync();
    }

    public async Task UpdateReservation(Reservation reservation)
    {
        context.Reservations.Update(reservation);
        await context.SaveChangesAsync();
    }

    public async Task DeleteReservation(Reservation reservation)
    {
        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync();
    }
}
