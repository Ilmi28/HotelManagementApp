using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Context;

namespace HotelManagementApp.Infrastructure.Loggers;

public class AuthDbLogger(AppDbContext context) : IDbLogger<UserDto>
{
    private readonly AppDbContext _context = context;

    public async Task Log(AccountOperationEnum operation, UserDto loggedObject)
    {
        var userLog = new UserLog
        {
            Operation = operation,
            UserId = loggedObject.Id,
            Date = DateTime.Now
        };

        _context.AccountLogs.Add(userLog);
        await _context.SaveChangesAsync();
    }
}
