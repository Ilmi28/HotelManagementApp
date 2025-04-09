using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string RefreshTokenHash { get; set; }
        public required string UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}
