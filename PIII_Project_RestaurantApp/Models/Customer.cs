using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PIII_Project_RestaurantApp;

namespace PIII_Project_RestaurantApp.Models
{
    public class Customer : User, IOrderManagement
    {
        private List<Order> _orderHistory;
        private List<OrderItem> _cartItems;
        private string _phone;
        private decimal _totalSpent;
        private readonly string _csvFilePath = @"..\..\..\Data\dishes.csv";

        public string CsvFilePath
        {
            get { return _csvFilePath; }
        }

        #region constructor
        public Customer(string name, string email = "", string phone = "") : base(name, email)
        {
            _orderHistory = new List<Order>();
            _cartItems = new List<OrderItem>();
            _totalSpent = 0;
            Name = name;
            Phone = phone;
            Email = email;

        }
        #endregion

        #region Properties


        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }


        public List<OrderItem> CartItem
        {
            get { return _cartItems; }
            set { _cartItems = value; }
        }
        public List<Order> OrderHistory
        {
            get { return _orderHistory; }
            private set { _orderHistory = value; }
        }
        #endregion


        #region Behavior for Cart
        public void AddToCart(Dish dish, int quantity = 1)
        {
            var existingItem = _cartItems.FirstOrDefault(item => item.DishName == dish.Name);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                _cartItems.Add(new OrderItem(dish.Name, dish.Price, 1));
            }
        }
        public List<OrderItem> GetCartItems()
        {
            return _cartItems;
        }
        public decimal GetCartTotal()
        {
            return _cartItems.Sum(item => item.SubTotal);
        }

        public void RemoveFromCart(OrderItem orderItem)
        {
            var item = _cartItems.FirstOrDefault(x => x.DishId == orderItem.DishId);
            if (item != null)
            {
                _cartItems.Remove(item);
            }

        }
        public void ClearCart()
        {
            _cartItems.Clear();
        }
        #endregion

        #region Behavior for Order
        public Order PlaceOrder()
        {
            // check cart
            if (_cartItems == null || !_cartItems.Any())
            {
                throw new ArgumentException("Cart cannot be empty");
            }

            // place an order
            Order order = new Order(this, _cartItems);

            // set order status
            order.UpdateStatus(OrderStatus.Pending);

            // Calculate order totals and update user spending amounts
            decimal orderTotal = order.Total;
            _totalSpent += orderTotal;

            // Add to order history
            _orderHistory.Add(order);

            // clear cart
            _cartItems.Clear();

            return order;
        }
        public void CancelOrder(int orderId)
        {
            Order order = null;
            for (int i = 0; i < _orderHistory.Count; i++)
            {
                if (_orderHistory[i].OrderId == orderId)
                {
                    order = _orderHistory[i];
                    break;
                }
            }

            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            if (order.Status == OrderStatus.Pending)
            {
                order.UpdateStatus(OrderStatus.Cancelled);
            }
            else
            {
                throw new InvalidOperationException("Only pending orders can be cancelled");
            }
        }
        public List<Order> ViewOrderHistory()
        {
            return _orderHistory;  //Go directly back to all order history
        }

        public List<Order> ViewOrderHistoryByDate(DateTime date)
        {
            List<Order> todayOrders = new List<Order>();

            for (int i = 0; i < _orderHistory.Count; i++)
            {
                if (_orderHistory[i].OrderDate.Date == date.Date)
                {
                    todayOrders.Add(_orderHistory[i]);
                }
            }

            return todayOrders;
        }
        public decimal GetTotalSpent()
        {
            return _totalSpent;
        }

        #endregion

        #region Implement interface
        // IMenuView Implement 
        public Dish GetDish(int id)
        {
            // TODO: 
            return null;
        }
        public List<Dish> GetAllDishes()
        {
            try
            {
                if (!File.Exists(_csvFilePath))
                {
                    throw new Exception("Menu doesn't exist");
                }
                List<Dish> getAllDishes = new List<Dish>();
                using (StreamReader reader = new StreamReader(_csvFilePath))
                {
                    // Skip header
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string[] values = reader.ReadLine().Split(',');
                        getAllDishes.Add(new Dish(
                            int.Parse(values[0]),     // Id
                            values[1],                // Name
                            decimal.Parse(values[2]),  // Price
                            (DishCategory)Enum.Parse(typeof(DishCategory), values[3]), // Category
                            values[5]                 // ImagePath
                        ));
                    }
                }
                return getAllDishes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading dishes: {ex.Message}");
                return new List<Dish>();
            }
        }

        public List<Dish> GetDishesByCategory(DishCategory category)
        {
            // TODO:
            return new List<Dish>();
        }
        // IOrderManagement Implement
        public Order GetOrderById(int orderId)
        {
            for (int i = 0; i < _orderHistory.Count; i++)
            {
                if (_orderHistory[i].OrderId == orderId)
                {
                    return _orderHistory[i];
                }
            }
            return null;
        }

        public List<Order> GetDailyOrders(DateTime date)
        {
            List<Order> dailyOrders = new List<Order>();

            for (int i = 0; i < _orderHistory.Count; i++)
            {
                if (_orderHistory[i].OrderDate.Date == date.Date)
                {
                    dailyOrders.Add(_orderHistory[i]);
                }
            }

            return dailyOrders;
        }

        public void UpdateOrderStatus(int orderId, OrderStatus status)
        {
            for (int i = 0; i < _orderHistory.Count; i++)
            {
                if (_orderHistory[i].OrderId == orderId)
                {
                    _orderHistory[i].UpdateStatus(status);
                    break;
                }
            }
        }

        #endregion

    }
}
