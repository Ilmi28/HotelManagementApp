using HotelManagementApp.Core.Enums;

namespace HotelManagementApp.Core.Models;

public class UserLog
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public AccountOperationEnum Operation { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
