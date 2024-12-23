    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIII_Project_RestaurantApp.Models
{
    public class Dish
    {

        private int _id;
        private string _name;
        private decimal _price;
        private string _imagePath;
        private DishCategory _category;
        private bool _isAvailable;
        private bool _isSelected;

        public Dish(int id, string name, decimal price, DishCategory category, string imagePath = "")
        {

            _id = id;
            _name = name;
            _price = price;
            _category = category;
            _imagePath = imagePath ?? "";
            _isAvailable = true;
        }

        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Name cannot be empty");
                _name = value;
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

        public string ImagePath
        {
            get { return _imagePath; }
            private set { _imagePath = value ?? ""; }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }
        public DishCategory Category
        {
            get { return _category; }
            private set { _category = value; }
        }

        public bool IsAvailable
        {
            get { return _isAvailable; }
            set { _isAvailable = value; }
        }



        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }
            Price = newPrice;

        }
        public void UpdateCategory(DishCategory dCategory)
        {
            Category = dCategory;
        }
        public void UpdateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            Name = name;
        }

        public void UpdateImage(string newImagePath)
        {
            _imagePath = newImagePath ?? "";
        }

        public void UpdateAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }
    }
}
