using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class DiscountNotFoundException(string message) : NotFoundException(message)
{

}
