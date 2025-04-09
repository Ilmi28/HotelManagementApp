using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Context;

namespace HotelManagementApp.Infrastructure.Loggers;

public class AuthDbLogger(HotelManagementAppDbContext context) : IDbLogger<UserDto>
{
    private readonly HotelManagementAppDbContext _context = context;

    public async Task Log(OperationEnum operation, UserDto loggedObject)
    {
        var userLog = new UserLog
        {
            Operation = operation,
            UserId = loggedObject.Id,
            Date = DateTime.Now
        };

        _context.UserLogs.Add(userLog);
        await _context.SaveChangesAsync();
    }
}
