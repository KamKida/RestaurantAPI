using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirment : IAuthorizationRequirement
    {

        public int MinimumRestaurantCreated { get;}

        public CreatedMultipleRestaurantsRequirment(int minimumRestaurantCreated)
        {
            MinimumRestaurantCreated = minimumRestaurantCreated;
        }
    }
}
