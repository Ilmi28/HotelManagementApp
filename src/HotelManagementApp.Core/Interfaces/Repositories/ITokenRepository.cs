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
        Task<bool> AddToken(Token token);
        Task<Token?> GetToken(string refreshToken);
    }
}
