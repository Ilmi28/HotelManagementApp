using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class LoyaltyRewardNotFoundException(string message) : NotFoundException(message)
{
    
}