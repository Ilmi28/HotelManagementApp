using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.API.Requests.MyAccount
{
    public class MyAccountChangePasswordRequest
    {
        [Required]
        public required string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.")]
        public required string NewPassword { get; set; }
    }
}
