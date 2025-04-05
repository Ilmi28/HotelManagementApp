using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.MyAccount.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {

        private readonly IUserManager _userManager;
        private readonly IDbLogger<UserDto> _logger;
        public ChangePasswordCommandHandler(IUserManager userManager, IDbLogger<UserDto> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    throw new UnauthorizedAccessException();
                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!result)
                    throw new UnauthorizedAccessException();
                await _logger.Log(OperationEnum.PasswordChange, user);
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred", ex);
            }
        }
    }
}
