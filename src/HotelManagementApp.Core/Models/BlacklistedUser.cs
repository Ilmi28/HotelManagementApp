using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Models
{
    public class BlacklistedUser
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public DateTime BlacklistedDate { get; set; } = DateTime.Now;
    }
}
