using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Hotel.GetById;

public class GetHotelByIdQueryHandler(
    IHotelRepository hotelRepository,
    IHotelImageRepository imageRepository,
    IFileService fileService) : IRequestHandler<GetHotelByIdQuery, HotelResponse>
{
    public async Task<HotelResponse> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        return new HotelResponse
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Address = hotel.Address,
            City = hotel.City.Name,
            Country = hotel.City.Country,
            Description = hotel.Description,
            PhoneNumber = hotel.PhoneNumber,
            Email = hotel.Email,
            Images = (await imageRepository.GetHotelImagesByHotelId(hotel.Id, cancellationToken))
                .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList(),
        };
    }
}
