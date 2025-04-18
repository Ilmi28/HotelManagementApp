using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Models;

namespace HotelManagementApp.Core.Interfaces.Loggers;

/// <summary>
/// Interface for logging database operations.
/// </summary>
/// <typeparam name="T">Type of logged object</typeparam>
/// <typeparam name="X">Type of operation enum</typeparam>
/// <typeparam name="Y">Type of log response</typeparam>
public interface IDbLogger<T, X, Y>
{
    Task Log(X operation, T type);
    Task<ICollection<Y>> GetLogs(T type);
}
