using HotelManagementApp.Application.CQRS.Cities.GetByCountry;
using HotelManagementApp.Application.CQRS.Cities.GetById;
using HotelManagementApp.Application.CQRS.Cities.GetCountries;
using HotelManagementApp.Application.CQRS.Cities.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/city")]
[ApiController]
public class CityController(IMediator mediator) : ControllerBase
{
    [HttpGet("get-by-country/{country}")]
    public async Task<IActionResult> GetCities(string country, CancellationToken ct)
    {
        var query = new GetCitiesByCountryQuery { Country = country };
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetCity(int id, CancellationToken ct)
    {
        var query = new GetCityByIdQuery { Id = id };
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpPatch("refresh")]
    public async Task<IActionResult> RefreshCities(CancellationToken ct)
    {
        var cmd = new RefreshCitiesCommand();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpGet("get-countries")]
    public async Task<IActionResult> GetCountries(CancellationToken ct)
    {
        var query = new GetCountriesQuery();
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}
