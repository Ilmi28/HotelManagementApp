using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.Delete
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
    {
        private readonly IUserManager _userManager;
        private readonly IDbLogger<UserDto> _logger; 
        public DeleteAccountCommandHandler(IUserManager userManager, IDbLogger<UserDto> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId)
                ?? throw new UnauthorizedAccessException();
                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!result)
                    throw new UnauthorizedAccessException();
                result = await _userManager.DeleteAsync(user);
                if (!result)
                    throw new Exception("User deletion failed");
                await _logger.Log(OperationEnum.Delete, user);
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured", ex);
            }
        }
    }
}
