using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.Update
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
    {
        private IUserManager _userManager;
        private IDbLogger<UserDto> _logger;
        public UpdateAccountCommandHandler(IUserManager userManager, IDbLogger<UserDto> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    throw new UnauthorizedAccessException();
                user.UserName = request.UserName;
                var result = await _userManager.UpdateAsync(user);
                if (!result)
                    throw new Exception("User update failed");
                await _logger.Log(OperationEnum.Update, user);
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured", ex);
            }

        }
    }
}
