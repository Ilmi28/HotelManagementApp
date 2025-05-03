using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.GetAll;

public class GetAllHotelsQueryHandler(
    IHotelRepository hotelRepository,
    IHotelImageRepository imageRepository,
    IFileService fileService) : IRequestHandler<GetAllHotelsQuery, ICollection<HotelResponse>>
{
    public async Task<ICollection<HotelResponse>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var hotelModels = await hotelRepository.GetAllHotels(cancellationToken);
        var response = await Task.WhenAll(hotelModels.Select(async h => new HotelResponse
        {
            Id = h.Id,
            Name = h.Name,
            Address = h.Address,
            City = h.City.Name,
            Country = h.City.Country,
            Description = h.Description,
            PhoneNumber = h.PhoneNumber,
            Email = h.Email,
            Images = (await imageRepository.GetHotelImagesByHotelId(h.Id, cancellationToken))
                .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList()
        }).ToList());
        return response;
    }
}
