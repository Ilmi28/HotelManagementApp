using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Cities.GetCountries;

public class GetCountriesQueryHandler(ICityRepository cityRepository) : IRequestHandler<GetCountriesQuery, ICollection<string>>
{
    public async Task<ICollection<string>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        return await cityRepository.GetCountries(cancellationToken);
    }
}

