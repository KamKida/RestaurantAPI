using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update, 
        Delete
    }

    public class ResourceOperationRequirment :IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation { get; }
        public ResourceOperationRequirment(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
    }
}
