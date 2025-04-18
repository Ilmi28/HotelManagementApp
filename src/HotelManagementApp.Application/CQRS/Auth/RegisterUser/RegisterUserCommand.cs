﻿using HotelManagementApp.Application.Responses.AuthResponses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Auth.RegisterUser;

public class RegisterUserCommand : IRequest<LoginRegisterResponse>
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string UserName { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.")]
    public required string Password { get; set; }
}
