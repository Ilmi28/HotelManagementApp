﻿using HotelManagementApp.Core.Models.AccountModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;

public interface IProfilePictureRepository
{
    Task<ProfilePicture?> GetProfilePicture(string userId, CancellationToken ct = default);
    Task AddProfilePicture(ProfilePicture profilePicture, CancellationToken ct = default);
    Task RemoveProfilePicture(string userId, CancellationToken ct = default);
}
