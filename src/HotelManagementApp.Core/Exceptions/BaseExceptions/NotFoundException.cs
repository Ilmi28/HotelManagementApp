namespace HotelManagementApp.Core.Exceptions.BaseExceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public NotFoundException(string message, string details) : base(message)
    {
        Details = details;
    }
    public string Details { get; set; } = string.Empty;
}
