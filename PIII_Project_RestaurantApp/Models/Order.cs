using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIII_Project_RestaurantApp.Models
{
    public class Order
    {
        private Customer _customer;
        private int _orderId;
        private List<OrderItem> _items;
        private OrderStatus _status;
        private DateTime _orderDate;
        private static int _globalOrderId = 1;

        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }
        #region Properties

        public Customer Customer
        {
            get { return _customer; }
            private set { _customer = value; }
        }
        public List<OrderItem> Items
        {
            get { return _items; }
            private set { _items = value; }
        }

        public OrderStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }
        public decimal Total
        {
            get
            {
                decimal total = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    total += Items[i].Price * Items[i].Quantity;
                }
                return total;
            }

        }
        #endregion
        #region Constructor
        public Order()
        {
            OrderId = _globalOrderId++;
            Items = new List<OrderItem>();
            OrderDate = DateTime.Now;
            Status = OrderStatus.Pending;
        }
        public Order(Customer customer) : this()
        {
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }
        public Order(Customer customer, List<OrderItem> orderItems) : this(customer)
        {
            if (orderItems != null)
            {
                foreach (var item in orderItems)
                {
                    _items.Add(new OrderItem(item.DishId, item.DishName, item.Price, item.Quantity));
                }
            }
        }
        #endregion
        public void UpdateStatus(OrderStatus newStatus)
        {
            if (Status == OrderStatus.Cancelled && newStatus != OrderStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot change status of cancelled order");
            }

            if (Status == OrderStatus.Completed && newStatus != OrderStatus.Completed)
            {
                throw new InvalidOperationException("Cannot change status of delivered order");
            }

            Status = newStatus;
        }   
    }
}
