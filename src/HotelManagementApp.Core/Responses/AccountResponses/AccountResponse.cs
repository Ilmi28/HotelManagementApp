using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Responses.AccountResponses
{
    public class AccountResponse
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}
