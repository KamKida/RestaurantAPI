using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;

namespace RestaurantAPI.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);

        string GenerateToken(LoginDto dto);
    }
}
