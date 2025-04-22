using HotelManagementApp.Core.Enums;

namespace HotelManagementApp.Core.Models.AccountModels;

public class AccountLog
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required AccountOperationEnum AccountOperation { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
