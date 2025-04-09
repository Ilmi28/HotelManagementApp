using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.API.Requests.MyAccount
{
    public class MyAccountUpdateRequest
    {
        [MinLength(3)]
        [MaxLength(50)]
        [Required]
        public required string UserName { get; set; }
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
