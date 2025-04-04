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
        Task AddToken(Token token);
        Task<Token?> GetToken(string refreshToken);
        Task<Token?> GetLastValidToken(string userId);
        Task RevokeToken(Token token);
    }
}
