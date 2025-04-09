using HotelManagementApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        Task AddToken(RefreshToken token);
        Task<RefreshToken?> GetToken(string refreshToken);
        Task<RefreshToken?> GetLastValidToken(string userId);
        Task RevokeToken(RefreshToken token);
    }
}
