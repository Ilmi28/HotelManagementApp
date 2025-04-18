﻿using HotelManagementApp.Core.Models;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface ITokenRepository
{
    Task AddToken(RefreshToken token, CancellationToken ct = default);
    Task<RefreshToken?> GetToken(string refreshToken, CancellationToken ct = default);
    Task<RefreshToken?> GetLastValidToken(string userId, CancellationToken ct = default);
    Task RevokeToken(RefreshToken token, CancellationToken ct = default);
}
