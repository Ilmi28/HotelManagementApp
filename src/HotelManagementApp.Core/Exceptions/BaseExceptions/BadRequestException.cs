namespace HotelManagementApp.Core.Exceptions.BaseExceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
    public BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public BadRequestException(string message, string details) : base(message)
    {
        Details = details;
    }
    public string Details { get; set; } = string.Empty;
}
