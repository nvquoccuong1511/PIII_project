using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIII_Project_RestaurantApp.Models
{
    public class DailySales
    {
        public DateTime Date { get; private set; }
        public decimal TotalSales { get; private set; }
        public int OrderCount { get; private set; }
        public List<Order> Orders { get; private set; }

        public DailySales(DateTime date)
        {
            Date = date;
            Orders = new List<Order>();
            TotalSales = 0;
            OrderCount = 0;
        }

        //Add orders and update statistics
        public void AddOrder(Order order)
        {
            if (order.OrderDate.Date != Date.Date)
            {
                throw new ArgumentException("Order date does not match daily sales date");
            }

            Orders.Add(order);
            TotalSales += order.Total;
            OrderCount++;
        }

        public List<Order> GetOrders()
        {
            return Orders;
        }

        public decimal GetTotalSales()
        {
            return TotalSales;
        }

        public int GetOrderCount()
        {
            return OrderCount;
        }

    }
}
