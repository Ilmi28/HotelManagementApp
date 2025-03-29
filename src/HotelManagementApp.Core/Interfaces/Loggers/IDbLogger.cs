using HotelManagementApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Loggers
{
    public interface IDbLogger<T>
    {
        Task Log(Operation operation, T loggedObject);
    }
}
