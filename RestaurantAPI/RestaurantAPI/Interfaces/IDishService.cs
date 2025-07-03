using RestaurantAPI.Models;
using System.Collections.Generic;

public interface IDishService
{
    int Create(int restaurantId, CreateDishDto dto);
    DishDto GetById(int restaurantId, int dishId);
    List<DishDto> GetAll(int restaurantId);
    void RemoveAll(int restaurantId);
}
