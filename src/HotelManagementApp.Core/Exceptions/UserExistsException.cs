using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Exceptions
{
    public class UserExistsException : Exception
    {
        public UserExistsException(string message) : base(message)
        {
        }
    }
}
