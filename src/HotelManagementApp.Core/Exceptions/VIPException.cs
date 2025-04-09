using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Exceptions
{
    public class VIPException : Exception
    {
        public VIPException(string message) : base(message)
        {
        }
    }
}
