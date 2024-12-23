using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIII_Project_RestaurantApp.Models
{
    public class OrderItem
    {
        private int _dishId;
        private string _dishName;
        private int _quantity;
        private decimal _price;
        private decimal _subTotal;

        public int DishId
        {
            get { return _dishId; }
            private set { _dishId = value; }
        }

        public string DishName
        {
            get { return _dishName; }
            private set { _dishName = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Quantity must be greater than 0");
                _quantity = value;
            }
        }

        public decimal Price
        {
            get { return _price; }
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative");
                _price = value;
            }
        }

        public decimal SubTotal
        {
            get { return _price * _quantity; }
        }

        public OrderItem(string dishName, decimal price, int quantity)
        {
            _dishName = dishName;
            _price = price;
            _quantity = quantity;
        }
        public OrderItem(int dishId, string dishName, decimal price, int quantity) : this(dishName, price, quantity)
        {
            _dishId = dishId;
        }
        private void CalculateSubTotal()
        {
            _subTotal = _price * _quantity;
        }
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }
            _quantity = newQuantity;
            CalculateSubTotal();

        }
    }
}
