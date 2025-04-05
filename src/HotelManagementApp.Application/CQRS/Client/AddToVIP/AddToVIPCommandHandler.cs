using HotelManagementApp.Core.Exceptions;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Client.AddToVIP
{
    public class AddToVIPCommandHandler : IRequestHandler<AddToVIPCommand>
    {
        private readonly IUserManager _userManager;
        private readonly IVIPUserRepository _vipUserRepository;
        public AddToVIPCommandHandler(IVIPUserRepository vipUserRepository, IUserManager userManager)
        {
            _vipUserRepository = vipUserRepository;
            _userManager = userManager;
        }
        public async Task Handle(AddToVIPCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    throw new UnauthorizedAccessException("User not found");
                var isUserVIP = await _vipUserRepository.IsUserVIP(request.UserId);
                if (isUserVIP)
                    throw new VIPException("User is already a VIP");
                await _vipUserRepository.AddUserToVIP(request.UserId);
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (VIPException) { throw; }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding user to blacklist", ex);
            }
        }
    }
}
