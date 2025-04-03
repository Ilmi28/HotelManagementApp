using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Responses.AccountResponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountById
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountResponse>
    {
        private readonly IUserManager _userManager;
        public GetAccountQueryHandler(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<AccountResponse> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new UnauthorizedAccessException();
            return new AccountResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles
            };
        }
    }
}
