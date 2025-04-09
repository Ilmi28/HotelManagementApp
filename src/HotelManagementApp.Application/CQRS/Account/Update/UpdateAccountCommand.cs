using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.Update
{
    public class UpdateAccountCommand : IRequest
    {
        public required string UserId { get; set; }
        [MinLength(3)]
        [MaxLength(50)]
        [Required]
        public required string UserName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
