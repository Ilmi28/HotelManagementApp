using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;

public interface IReservationRepository
{
    Task AddReservation(Reservation reservation);
    Task<Reservation?> GetReservationById(int id);
    Task<ICollection<Reservation>> GetAllReservations();
    Task<ICollection<HotelRoom>> GetReservationsByRoomId(int roomId);
    Task<ICollection<Reservation>> GetReservationsByHotelId(int hotelId);
    Task UpdateReservation(Reservation reservation);
    Task DeleteReservation(Reservation reservation);
}
