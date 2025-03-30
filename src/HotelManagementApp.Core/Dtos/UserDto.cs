using HotelManagementApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Dtos
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required IList<string> Roles { get; set; }
    }
}
