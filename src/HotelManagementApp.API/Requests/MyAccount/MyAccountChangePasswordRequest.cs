using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.API.Requests.MyAccount
{
    public class MyAccountChangePasswordRequest
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
