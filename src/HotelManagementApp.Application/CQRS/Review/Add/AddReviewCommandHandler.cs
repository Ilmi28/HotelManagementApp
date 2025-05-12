using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Review.Add;

public class AddReviewCommandHandler(
    IUserManager userManager, 
    IHotelReviewRepository reviewRepository,
    IHotelRepository hotelRepository,
    IReservationRepository reservationRepository) : IRequestHandler<AddReviewCommand>
{
    public async Task Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException();
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var canWriteReview = false;
        var reservations = await reservationRepository.GetReservationsByGuestId(user.Id, cancellationToken);
        foreach (var reservation in reservations)
        {
            if (reservation.Order.Status is OrderStatusEnum.Completed &&
                reservation.To < DateOnly.FromDateTime(DateTime.Now))
            {
                canWriteReview = true;
                break;
            }
        }
        if (!canWriteReview)
            throw new InvalidOperationException($"User with id {user.Id} can't write review for hotel with id {request.HotelId} because he/she has not completed any reservation for this hotel");
        var review = new HotelReview
        {
            UserId = user.Id,
            UserName = request.UserName,
            Created = DateTime.Now,
            Review = request.Review,
            Rating = request.Rating,
            Hotel = hotel
        };
        await reviewRepository.AddReview(review, cancellationToken);
    }
}