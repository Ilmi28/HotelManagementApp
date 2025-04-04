using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.Create
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
    {
        private IUserManager _userManager;
        private IDbLogger<UserDto> _logger;
        public CreateAccountCommandHandler(IUserManager userManager, IDbLogger<UserDto> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            var dbUser = await _userManager.FindByEmailAsync(request.Email);
            if (dbUser != null)
                throw new UserAlreadyExistsException("User with this email already exists.");
            dbUser = await _userManager.FindByNameAsync(request.UserName);
            if (dbUser != null)
                throw new UserAlreadyExistsException("User with this username already exists.");
            try
            {
                var user = new UserDto
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = request.UserName,
                    Email = request.Email,
                    Roles = request.Roles
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result)
                    throw new Exception("User creation failed");
                await _logger.Log(OperationEnum.Create, user);
            }
            catch(Exception ex) { throw new Exception("Unexpected error occured or invalid role.", ex); }
        }
    }
}
