using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIII_Project_RestaurantApp.Models
{
    internal interface IMenuManager
    {
        void AddDish(Dish dish);
        //void RemoveDish(Dish dish);
        void RemoveSelectedDishes();
        void UpdateDish(Dish dish);
    }
}
