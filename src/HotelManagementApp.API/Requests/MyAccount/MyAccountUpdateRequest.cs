using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.API.Requests.MyAccount
{
    public class MyAccountUpdateRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
