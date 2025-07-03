using RestaurantAPI.Context;
using System.Collections.Generic;
using RestaurantAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using RestaurantAPI.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RestaurantAPI.Interfaces;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Authorization;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext restaurantDbContext, IMapper mapper, ILogger<RestaurantService> logger
            ,IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);


            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");


            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _restaurantDbContext
               .Restaurants
               .Include(r => r.Address)
               .Include(r => r.Dishes)
               .ToList();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
            return restaurantsDtos;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _restaurantDbContext.Restaurants.Add(restaurant);
            _restaurantDbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Delete(int id)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);


            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceOperationRequirment(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidExeption();
            }

            _restaurantDbContext.Remove(restaurant);
            _restaurantDbContext.SaveChanges();

        }

        public void Update(int id, UpdateRestaurantDto dto)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found.");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceOperationRequirment(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidExeption();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDDelivery = dto.HasDDelivery;

            _restaurantDbContext.SaveChanges();
        }
    }
}
