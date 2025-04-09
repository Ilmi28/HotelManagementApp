namespace HotelManagementApp.Core.Exceptions.BaseExceptions;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
    public ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public ConflictException(string message, string details) : base(message)
    {
        Details = details;
    }
    public string Details { get; set; } = string.Empty;
}
