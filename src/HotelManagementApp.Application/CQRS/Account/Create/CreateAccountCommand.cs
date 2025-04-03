using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.Create
{
    public class CreateAccountCommand : IRequest
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
        public List<string> Roles { get; set; } = new List<string>();
    }
}
