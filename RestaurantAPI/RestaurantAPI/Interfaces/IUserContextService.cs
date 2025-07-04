﻿using System.Security.Claims;

namespace RestaurantAPI.Interfaces
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}