using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIII_Project_RestaurantApp.Models
{
        internal interface IMenuView
        {
            Dish GetDish(int id);
            List<Dish> GetAllDishes();
            List<Dish> GetDishesByCategory(DishCategory category);
        }
}
