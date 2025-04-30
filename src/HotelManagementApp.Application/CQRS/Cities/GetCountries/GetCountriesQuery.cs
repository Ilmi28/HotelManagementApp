using MediatR;

namespace HotelManagementApp.Application.CQRS.Cities.GetCountries;

public class GetCountriesQuery : IRequest<ICollection<string>>
{

}
