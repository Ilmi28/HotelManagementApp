using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.UpdateProfilePicture
{
    public class UpdateProfilePictureCommand
    {
        public required string UserId { get; set; }
        public required IFormFile ProfilePicture { get; set; } 
    }
}
