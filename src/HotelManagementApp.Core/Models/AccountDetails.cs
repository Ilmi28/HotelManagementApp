namespace HotelManagementApp.Core.Models;

public class AccountDetails
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string Surname { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string PostalCode { get; set; }
    public required string PhoneNumber { get; set; }
}
