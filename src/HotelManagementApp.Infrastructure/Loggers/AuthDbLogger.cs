using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Loggers;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Loggers;

public class AuthDbLogger(AppDbContext context) : IDbLogger<UserDto, AccountOperationEnum, UserLog>
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

        _context.AccountHistory.Add(userLog);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<UserLog>> GetLogs(UserDto user)
    {
        return await _context.AccountHistory
            .Where(x => x.UserId == user.Id)
            .ToListAsync();
    }

}
