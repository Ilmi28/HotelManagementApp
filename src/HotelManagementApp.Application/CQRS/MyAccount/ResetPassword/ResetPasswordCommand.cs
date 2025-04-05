using MediatR;

namespace HotelManagementApp.Application.CQRS.MyAccount.ResetPassword
{
    public class ResetPasswordCommand : IRequest
    {
        public required string ResetPasswordToken { get; set; }
        public required string NewPassword { get; set; }
    }
}
