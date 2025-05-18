namespace HotelManagementApp.Application.Responses.AccountResponses;

public class AccountResponse()
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public ICollection<string> Roles { get; set; } = [];
    public required string ProfilePicture { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;
}
