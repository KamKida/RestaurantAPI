using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using RestaurantAPI.Context;
using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        public readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }


                if (!_dbContext.Restaurants.Any())
                {
                    var restraunts = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restraunts);
                    _dbContext.SaveChanges();
                }
            }
        }
            

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Fodd",
                    Description =
                        "Restaurant init.",
                    ContactEmail = "contact@kfc.com",
                    HasDDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Nashville Hot Chicken",
                            Price = 10.30M,
                        },

                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Price = 5.30M,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-001"
                    }
                },

                new Restaurant()
                {
                    Name = "KFC 2",
                    Category = "Fast Fodd",
                    Description =
                        "Restaurant init. It is.",
                    ContactEmail = "contact@kfc2.com",
                    HasDDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Nashville Hot Chicken - Spicy",
                            Price = 22.30M,
                        },

                        new Dish()
                        {
                            Name = "Chicken Nuggets - Extra",
                            Price = 10.30M,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Rzeszów",
                        Street = "Jkaś 2",
                        PostalCode = "33-333"
                    }
                }
            }; 
            
            return restaurants;
        }
    }
}
