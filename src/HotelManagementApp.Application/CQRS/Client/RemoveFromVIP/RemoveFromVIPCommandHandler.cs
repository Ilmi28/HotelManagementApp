using HotelManagementApp.Core.Exceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Client.RemoveFromVIP
{
    public class RemoveFromVIPCommandHandler : IRequestHandler<RemoveFromVIPCommand>
    {
        private readonly IVIPUserRepository _vipUserRepository;
        private readonly IUserManager _userManager;
        public RemoveFromVIPCommandHandler(IVIPUserRepository vipUserRepository, IUserManager userManager)
        {
            _vipUserRepository = vipUserRepository;
            _userManager = userManager;
        }
        public async Task Handle(RemoveFromVIPCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    throw new UnauthorizedAccessException();
                var isUserVIP = await _vipUserRepository.IsUserVIP(request.UserId);
                if (!isUserVIP)
                    throw new VIPException("User is not a VIP");
                await _vipUserRepository.RemoveUserFromVIP(request.UserId);
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (VIPException) { throw; }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing user from VIP", ex);
            }
        }
    }
}
