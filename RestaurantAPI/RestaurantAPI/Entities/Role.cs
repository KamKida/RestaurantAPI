﻿using System.Collections.Generic;

namespace RestaurantAPI.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
