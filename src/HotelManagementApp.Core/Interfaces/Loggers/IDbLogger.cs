using HotelManagementApp.Core.Enums;

namespace HotelManagementApp.Core.Interfaces.Loggers;

public interface IDbLogger<T>
{
    Task Log(AccountOperationEnum operation, T loggedObject);
}
