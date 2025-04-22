using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Models.AccountModels;

namespace HotelManagementApp.Core.Interfaces.Loggers;

public interface IAccountDbLogger : IDbLogger<UserDto, AccountOperationEnum, AccountLog>
{

}
