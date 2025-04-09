using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Models
{
    public class ProfilePicture
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string FileName { get; set; }
    }
}
