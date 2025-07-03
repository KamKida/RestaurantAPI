using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Context;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class AccountService :  IAccountService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(RestaurantDbContext restaurantDbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _restaurantDbContext = restaurantDbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }


        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PassworddHash = hashedPassword;

            _restaurantDbContext.Users.Add(newUser);
            _restaurantDbContext.SaveChanges();
        }


        public string GenerateToken(LoginDto dto)
        {
            var user = _restaurantDbContext.Users
                .Include(r => r.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if(user is null)
            {
                throw new BadRequestException("Invalid username or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PassworddHash, dto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password.");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
                
            };

            if (!String.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(
                new Claim("Nationality", user.Nationality)
                );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);


            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred
                );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);



        }
    }
}
