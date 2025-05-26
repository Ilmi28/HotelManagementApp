using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
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
        var response = new List<HotelResponse>();
        foreach (var hotelModel in hotelModels)
        {
            response.Add(new HotelResponse
            {
                Id = hotelModel.Id,
                Name = hotelModel.Name,
                Address = hotelModel.Address,
                City = hotelModel.City.Name,
                Country = hotelModel.City.Country,
                Description = hotelModel.Description,
                PhoneNumber = hotelModel.PhoneNumber,
                Email = hotelModel.Email,
                Images = (await imageRepository.GetHotelImagesByHotelId(hotelModel.Id, cancellationToken))
                    .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList()
            });
        }

        return response;
    }
}
