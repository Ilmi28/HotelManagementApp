namespace HotelManagementApp.Core.Exceptions.BaseExceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }
    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public ForbiddenException(string message, string details) : base(message)
    {
        Details = details;
    }
    public string Details { get; set; } = string.Empty;
}
