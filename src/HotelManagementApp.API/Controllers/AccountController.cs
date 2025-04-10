﻿using HotelManagementApp.Application.CQRS.Account.Create;
using HotelManagementApp.Application.CQRS.Account.Delete;
using HotelManagementApp.Application.CQRS.Account.GetAccountById;
using HotelManagementApp.Application.CQRS.Account.GetAccountsInRole;
using HotelManagementApp.Application.CQRS.Account.Update;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/account")]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand cmd)
    {
        await mediator.Send(cmd);
        return Created();
    }

    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountCommand cmd)
    {
        await mediator.Send(cmd);
        return NoContent();
    }

    [HttpPost("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountCommand cmd)
    {
        await mediator.Send(cmd);
        return NoContent();
    }

    [HttpGet("by-id/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccount(string id)
    {
        var account = await mediator.Send(new GetAccountQuery { UserId = id });
        return Ok(account);
    }

    [HttpGet("by-role/{role}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAccountsWithRole(string role)
    {
        var account = await mediator.Send(new GetAccountsInRoleQuery { RoleName = role });
        return Ok(account);
    }
}
