using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetRoomTypes;

public class GetRoomTypesQueryHandler(IRoomRepository roomRepository) : IRequestHandler<GetRoomTypesQuery, ICollection<RoomTypeResponse>>
{
    public async Task<ICollection<RoomTypeResponse>> Handle(GetRoomTypesQuery request, CancellationToken cancellationToken)
    {
        var roomTypes = await roomRepository.GetRoomTypes(cancellationToken);
        return roomTypes.Select(rt => new RoomTypeResponse
        {
            Id = rt.Id,
            Name = rt.Name.ToString()
        }).ToList();
    }
}
