using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Models;

namespace HotelManagementApp.Application.Responses.AccountResponses;

public class AccountLogResponse
{
    public required string Operation { get; set; }
    public required DateTime OperationDate { get; set; }
}
