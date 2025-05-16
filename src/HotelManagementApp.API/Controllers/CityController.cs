using HotelManagementApp.Application.CQRS.Cities.GetByCountry;
using HotelManagementApp.Application.CQRS.Cities.GetById;
using HotelManagementApp.Application.CQRS.Cities.GetCountries;
using HotelManagementApp.Application.CQRS.Cities.Refresh;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Route("api/city")]
[Authorize]
[ApiController]
public class CityController(IMediator mediator) : ControllerBase
{
    
    /// <summary>
    /// Returns all cities in a specified country
    /// </summary>
    /// <response code="200">Returns list of cities in the country</response>
    [HttpGet("get-by-country/{country}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCities(string country, CancellationToken ct)
    {
        var query = new GetCitiesByCountryQuery { Country = country };
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }

    /// <summary>
    /// Returns city by its ID
    /// </summary>
    /// <response code="200">Returns the requested city</response>
    [HttpGet("get-by-id/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCity(int id, CancellationToken ct)
    {
        var query = new GetCityByIdQuery { Id = id };
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }

    /// <summary>
    /// Refreshes the cities data
    /// </summary>
    /// <response code="204">Cities refreshed successfully</response>
    [HttpPatch("refresh")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RefreshCities(CancellationToken ct)
    {
        var cmd = new RefreshCitiesCommand();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Returns list of all available countries
    /// </summary>
    /// <response code="200">Returns list of countries</response>
    [HttpGet("get-countries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCountries(CancellationToken ct)
    {
        var query = new GetCountriesQuery();
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}
