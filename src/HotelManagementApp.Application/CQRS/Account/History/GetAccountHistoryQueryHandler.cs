using HotelManagementApp.Application.Responses.AccountResponses;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Loggers;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.History;

public class GetAccountHistoryQueryHandler(
    IUserManager userManager,
    IAccountDbLogger logger)
    : IRequestHandler<GetAccountHistoryQuery, ICollection<AccountLogResponse>>
{
    public async Task<ICollection<AccountLogResponse>> Handle(GetAccountHistoryQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId)
            ?? throw new UnauthorizedAccessException();
        var logs = await logger.GetLogs(user);
        return logs.Select(log => new AccountLogResponse
        {
            Operation = log.AccountOperation.ToString(),
            OperationDate = log.Date,
        }).ToList();
    }
}
