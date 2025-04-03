using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.ResetPassword
{
    public class ResetPasswordCommand : IRequest
    {
        public required string ResetPasswordToken { get; set; }
        public required string NewPassword { get; set; }
    }
}
