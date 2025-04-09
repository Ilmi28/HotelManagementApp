using HotelManagementApp.Core.Exceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Client.RemoveFromBlacklist
{
    public class RemoveFromBlacklistCommandHandler : IRequestHandler<RemoveFromBlacklistCommand>
    {
        private readonly IBlacklistedUserRepository _blacklistedUserRepository;
        private readonly IUserManager _userManager;
        public RemoveFromBlacklistCommandHandler(IBlacklistedUserRepository blacklistedUserRepository, IUserManager userManager)
        {
            _blacklistedUserRepository = blacklistedUserRepository;
            _userManager = userManager;
        }
        public async Task Handle(RemoveFromBlacklistCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    throw new UnauthorizedAccessException("User not found");
                var isUserBlacklisted = await _blacklistedUserRepository.IsUserBlacklisted(request.UserId);
                if (!isUserBlacklisted)
                    throw new BlacklistException("User is not blacklisted");
                await _blacklistedUserRepository.RemoveUserFromBlacklist(request.UserId);
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (BlacklistException) { throw; }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing user from blacklist", ex);
            }
        }
    }
}
