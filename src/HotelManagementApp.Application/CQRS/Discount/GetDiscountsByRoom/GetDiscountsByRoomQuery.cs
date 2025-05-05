using HotelManagementApp.Application.Responses.DiscountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.GetDiscountsByRoom;

public class GetDiscountsByRoomQuery : IRequest<ICollection<RoomDiscountResponse>>  
{
    public required int RoomId { get; set; }
}
