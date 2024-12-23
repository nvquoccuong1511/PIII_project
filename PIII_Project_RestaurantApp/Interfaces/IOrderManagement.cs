using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIII_Project_RestaurantApp.Models;

namespace PIII_Project_RestaurantApp.Models
{
    internal interface IOrderManagement
    {
        Order GetOrderById(int orderId);
        List<Order> GetDailyOrders(DateTime date);
        void UpdateOrderStatus(int orderId, OrderStatus status);
    }
}
