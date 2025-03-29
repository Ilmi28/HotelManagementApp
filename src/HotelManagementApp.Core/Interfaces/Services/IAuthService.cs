using HotelManagementApp.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(string identityToken, string refreshToken)> LoginUser(string email, string password);
        Task<(string identityToken, string refreshToken)> RegisterUser(string userName, string email, string password, List<string> roles);
        Task<string> RefreshToken(string refreshToken);
    }
}
