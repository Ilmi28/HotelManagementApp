using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Exceptions
{
    public class BlacklistException : Exception
    {
        public BlacklistException(string message) : base(message)
        {
        }
    }
}
