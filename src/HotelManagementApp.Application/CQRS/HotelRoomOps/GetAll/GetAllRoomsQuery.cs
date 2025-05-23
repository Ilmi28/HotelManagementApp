﻿using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetAll;

public class GetAllRoomsQuery : IRequest<ICollection<RoomResponse>>
{

}
