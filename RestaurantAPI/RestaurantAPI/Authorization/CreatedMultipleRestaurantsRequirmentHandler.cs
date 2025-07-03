using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Context;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirmentHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirment>
    {
        private readonly RestaurantDbContext _restaurantDbContext;

        public CreatedMultipleRestaurantsRequirmentHandler(RestaurantDbContext restaurantDbContext)
        {
            _restaurantDbContext = restaurantDbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirment requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

           var createdRestaurantsCount = _restaurantDbContext
                .Restaurants
                .Count(r => r.CreatedById == userId);

            if(createdRestaurantsCount >= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
            
        }
    }
}
