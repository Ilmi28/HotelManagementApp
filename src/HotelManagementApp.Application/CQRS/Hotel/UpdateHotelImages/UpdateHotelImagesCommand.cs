using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelManagementApp.Application.CQRS.Hotel.UpdateHotelImages;

public class UpdateHotelImagesCommand : IRequest
{
    public int HotelId { get; set; }
    public List<IFormFile> HotelImages { get; set; } = new List<IFormFile>();
}
