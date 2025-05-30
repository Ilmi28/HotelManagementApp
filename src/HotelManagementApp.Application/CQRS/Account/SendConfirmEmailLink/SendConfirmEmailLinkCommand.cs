﻿using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.SendConfirmEmailLink;

public class SendConfirmEmailLinkCommand : IRequest
{
    public required string UserId { get; set; }
}
