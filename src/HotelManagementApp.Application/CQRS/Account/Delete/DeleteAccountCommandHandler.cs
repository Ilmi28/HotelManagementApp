using HotelManagementApp.Core.Dtos;
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
            throw new NotImplementedException();
        }
    }
}
