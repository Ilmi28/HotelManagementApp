using HotelManagementApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Models
{
    public class UserLog
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public OperationEnum Operation { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
