﻿using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.Update;

public class UpdateRoomCommandHandler(IRoomRepository roomRepository, IHotelRepository hotelRepository) : IRequestHandler<UpdateRoomCommand>
{
    public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        _ = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        var hotelModel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var roomModel = new HotelRoom
        {
            Id = request.RoomId,
            RoomName = request.RoomName,
            RoomType = request.RoomType,
            Price = request.Price,
            Description = request.Description,
            Hotel = hotelModel
        };
        await roomRepository.UpdateRoom(roomModel, cancellationToken);
    }
}
