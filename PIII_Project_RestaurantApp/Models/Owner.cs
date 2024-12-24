using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PIII_Project_RestaurantApp;

namespace PIII_Project_RestaurantApp.Models
{
    public class Owner : User, IMenuManager
    {
        private static List<Dish> _dishes;
        private static List<Order> _orders;
        private static readonly string _csvDishesFilePath = @"..\..\..\Data\dishes.csv";
        private static readonly string _ordersFilePath = @"..\..\..\Data\orders.csv";
        private static readonly string _itemsFilePath = @"..\..\..\Data\order_items.csv";
        private static readonly string _customersFilePath = @"..\..\..\Data\order_items.csv";

        public Owner(string name, string email) : base(name, email)
        {
            _dishes = LoadDishesFromCsv();
            _orders = LoadOrdersFromCsv();
        }

        // Properties
        public List<Dish> Dishes
        {
            get { return _dishes; }
            private set { _dishes = value; }
        }


        public List<Order> GetAllOrders()
        {
            // Return empty list if orders is null
            if (_orders == null)
            {
                return new List<Order>();
            }

            // Return a new list containing all orders
            List<Order> allOrders = new List<Order>();
            for (int i = 0; i < _orders.Count; i++)
            {
                allOrders.Add(_orders[i]);
            }

            return allOrders;
        }
        public List<Order> GetDailyOrders(DateTime date)
        {
            // Return empty list if orders is null
            if (_orders == null)
            {
                return new List<Order>();
            }

            // Create new list for filtered orders
            List<Order> dailyOrders = new List<Order>();

            // Loop through each order and check date
            for (int i = 0; i < _orders.Count; i++)
            {
                if (_orders[i].OrderDate.Date == date.Date)
                {
                    dailyOrders.Add(_orders[i]);
                }
            }

            return dailyOrders;
        }



        // Method to get count of selected dishes

        public int GetSelectedDishesCount()
        {
            int count = 0;
            for (int i = 0; i < _dishes.Count; i++)
            {
                if (_dishes[i].IsSelected)
                {
                    count++;
                }
            }
            return count;
        }

        // IMenuManager Implementation
        public void AddDish(Dish dish)
        {
            if (dish == null)
            {
                throw new ArgumentNullException("Dish cannot be null");
            }

            // Check if dish with same ID already exists
            if (_dishes.Any(d => d.Id == dish.Id))
            {
                throw new ArgumentException("Dish with this ID already exists");
            }

            _dishes.Add(dish);
            SaveDishesToCsv(_dishes);
        }
        public void RemoveSelectedDishes()
        {
            List<Dish> dishesToRemove = new List<Dish>();

            for (int i = 0; i < _dishes.Count; i++)
            {
                if (_dishes[i].IsSelected)
                {
                    dishesToRemove.Add(_dishes[i]);
                }
            }

            foreach (Dish dish in dishesToRemove)
            {
                _dishes.Remove(dish);
            }
        }
        public void RemoveDish(Dish dish)
        {
            if (dish != null)
            {
                _dishes.Remove(dish);
                SaveDishesToCsv(_dishes);
            }
        }

        public void UpdateDish(Dish dish)
        {
            if (dish == null)
            {
                throw new ArgumentNullException("Dish cannot be null");
            }

            foreach (Dish d in _dishes)
            {
                if (d.Id == dish.Id)
                {
                    // Update the existing dish's properties
                    d.UpdatePrice(dish.Price);
                    d.UpdateImage(dish.ImagePath);
                    d.UpdateAvailability(dish.IsAvailable);
                    return;
                }
            }

            throw new ArgumentException("Dish not found");
        }
        

        // Additional helpful methods
        public Dish GetDishById(int id)
        {
            foreach (Dish dish in _dishes)
            {
                if (dish.Id == id)
                {
                    return dish;
                }
            }
            return null;
        }
        public List<Dish> GetDishes()
        {
            return _dishes;
        }
        public List<Dish> GetDishesByCategory(DishCategory category)
        {
            List<Dish> categoryDishes = new List<Dish>();
            foreach (Dish dish in _dishes)
            {
                if (dish.Category == category)
                {
                    categoryDishes.Add(dish);
                }
            }
            return categoryDishes;
        }
        #region Update Methods
        public void UpdateDish(int dishId, string name, decimal price, bool isAvailable, DishCategory category, string imagePath)
        {
            Dish dish = GetDishById(dishId);
            if (dish != null)
            {
                dish.UpdateName(name);
                dish.UpdatePrice(price);
                dish.UpdateAvailability(isAvailable);
                dish.UpdateCategory(category);
                dish.UpdateImage(imagePath);
                SaveDishesToCsv(_dishes);
                // Reload dishes to ensure e
                Dishes = LoadDishesFromCsv();
            }
        }

        //public bool UpdateDishName(Dish dish, string newName)
        //{
        //    if (dish != null)
        //    {
        //        try
        //        {
        //            dish.UpdateName(newName);
        //            SaveDishesToCsv(_dishes);
        //            return true;
        //        }
        //        catch (ArgumentException)
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}
        //public bool UpdateDishPrice(decimal newPrice, Dish dish)
        //{
        //    if (dish != null)
        //    {
        //        try
        //        {
        //            dish.UpdatePrice(newPrice);
        //            SaveDishesToCsv(_dishes); // save after updating
        //            return true;
        //        }
        //        catch (ArgumentException)
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}
        //public void UpdateDishImage(string newImagePath, Dish dish)
        //{
        //    dish.UpdateImage(newImagePath);
        //    SaveDishesToCsv(_dishes);  // Save after updating
        //    //_dishes = LoadDishesFromCsv();
        //}
        //public bool UpdateDishCategory(Dish dish, DishCategory newCategory)
        //{
        //    if (dish != null)
        //    {
        //        try
        //        {
        //            dish.UpdateCategory(newCategory);
        //            SaveDishesToCsv(_dishes);
        //            return true;
        //        }
        //        catch (ArgumentException)
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}
        //public bool UpdateDishAvailability(bool isAvailable, Dish dish)
        //{
        //    if (dish != null)
        //    {
        //        dish.UpdateAvailability(isAvailable);
        //        SaveDishesToCsv(_dishes);  // Save after updating
        //        return true;
        //    }
        //    return false;
        //}
        #endregion
        #region Generate data
        public static List<Customer> GenerateSampleCustomers()
        {
            List<Customer> customers = new List<Customer>
            {
                   new Customer("John Smith", "john.smith@email.com", "555-0101"),
                   new Customer("Sarah Johnson", "sarah.j@email.com", "555-0102"),
                   new Customer("Michael Williams", "michael.w@email.com", "555-0103"),
                   new Customer("Emma Brown", "emma.b@email.com", "555-0104"),
                   new Customer("James Wilson", "james.w@email.com", "555-0105"),
                   new Customer("Emily Davis", "emily.d@email.com", "555-0106"),
                   new Customer("William Garcia", "william.g@email.com", "555-0107"),
                   new Customer("Isabella Martinez", "isabella.m@email.com", "555-0108"),
                   new Customer("Alexander Anderson", "alex.a@email.com", "555-0109"),
                   new Customer("Olivia Taylor", "olivia.t@email.com", "555-0110"),
                   new Customer("Daniel Thomas", "daniel.t@email.com", "555-0111"),
                   new Customer("Sophie Moore", "sophie.m@email.com", "555-0112"),
                   new Customer("David Jackson", "david.j@email.com", "555-0113"),
                   new Customer("Victoria White", "victoria.w@email.com", "555-0114"),
                   new Customer("Joseph Harris", "joseph.h@email.com", "555-0115"),
                   new Customer("Charlotte Clark", "charlotte.c@email.com", "555-0116"),
                   new Customer("Benjamin Lewis", "ben.l@email.com", "555-0117"),
                   new Customer("Ava Rodriguez", "ava.r@email.com", "555-0118"),
                   new Customer("Henry Lee", "henry.l@email.com", "555-0119"),
                   new Customer("Mia Walker", "mia.w@email.com", "555-0120")
            };

            return customers;
        }
        // this is used in case file path has issue
        private static List<Dish> GenerateRestaurantDishes()
        {
            List<Dish> dishes = new List<Dish>
            {
                // Starters
                new Dish(1, "Garlic Bread", 5.99m, DishCategory.Starter, "Starter/Garlic_Bread.jpg"),
                new Dish(2, "Buffalo Wings", 11.99m, DishCategory.Starter, "Starter/Buffalo_Wings.jpg"),
                new Dish(3, "Calamari Rings", 12.99m, DishCategory.Starter, "Starter/Calamari_Rings.jpg"),
                new Dish(4, "Bruschetta", 8.99m, DishCategory.Starter, "Starter/Bruschetta.jpg"),
                new Dish(5, "Soup of the Day", 6.99m, DishCategory.Starter, "Starter/Soup_of_the_Day.jpg"),
                new Dish(6, "Prawn Cocktail", 10.99m, DishCategory.Starter, "Starter/Prawn_Cocktail.jpg"),

                // Mains
                new Dish(7, "Grilled Salmon", 24.99m, DishCategory.Main, "Main/Grilled_Salmon.jpg"),
                new Dish(8, "Ribeye Steak", 29.99m, DishCategory.Main, "Main/Ribeye_Steak.jpg"),
                new Dish(9, "Lamb Chops", 28.99m, DishCategory.Main, "Main/Lamb_Chops.jpg"),
                new Dish(10, "Chicken Alfredo", 18.99m, DishCategory.Main, "Main/Chicken_Alfredo.jpg"),
                new Dish(11, "BBQ Ribs", 26.99m, DishCategory.Main, "Main/BBQ_Ribs.jpg"),
                new Dish(12, "Vegetable Lasagna", 16.99m, DishCategory.Main, "Main/Vegetable_Lasagna.jpg"),
                new Dish(13, "Beef Burger", 15.99m, DishCategory.Main, "Main/Beef_Burger.jpg"),
                new Dish(14, "Fish and Chips", 17.99m, DishCategory.Main, "Main/Fish_and_Chips.jpg"),

                // Desserts
                new Dish(15, "Chocolate Cake", 8.99m, DishCategory.Dessert, "Dessert/chocolate_cake.jpg"),
                new Dish(16, "Cheesecake", 7.99m, DishCategory.Dessert, "Dessert/Cheesecake.jpg"),
                new Dish(17, "Apple Pie", 6.99m, DishCategory.Dessert, "Dessert/apple_pie.jpg"),
                new Dish(18, "Tiramisu", 8.99m, DishCategory.Dessert, "Dessert/Tiramisu.jpg"),
                new Dish(19, "Ice Cream Sundae", 5.99m, DishCategory.Dessert, "Dessert/Ice_Cream.jpg"),
                new Dish(20, "Crème Brûlée", 7.99m, DishCategory.Dessert, "Dessert/Crème_Brûlée.jpg")
            };

            return dishes;
        }
        private static List<Order> GenerateSampleOrders()
        {
            List<Order> orders = new List<Order>();
            Random random = new Random();
            DateTime today = DateTime.Now;

            // Load customers first
            List<Customer> customers = LoadCustomersFromCsv();

            // Sample dishes
            List<Dish> sampleDishes = new List<Dish>
    {
        new Dish(1, "Garlic Bread", 5.99m, DishCategory.Starter),
        new Dish(2, "Buffalo Wings", 11.99m, DishCategory.Starter),
        new Dish(3, "Calamari Rings", 12.99m, DishCategory.Starter),
        new Dish(4, "Bruschetta", 8.99m, DishCategory.Starter),
        new Dish(5, "Soup of the Day", 6.99m, DishCategory.Starter),
        new Dish(6, "Prawn Cocktail", 10.99m, DishCategory.Starter),
        new Dish(7, "Grilled Salmon", 24.99m, DishCategory.Main),
        new Dish(8, "Ribeye Steak", 29.99m, DishCategory.Main),
        new Dish(9, "Chicken Alfredo", 18.99m, DishCategory.Main),
        new Dish(10, "Fish and Chips", 17.99m, DishCategory.Main),
        new Dish(11, "BBQ Ribs", 26.99m, DishCategory.Main),
        new Dish(12, "Vegetable Lasagna", 16.99m, DishCategory.Main),
        new Dish(13, "Beef Burger", 15.99m, DishCategory.Main),
        new Dish(14, "Lamb Chops", 28.99m, DishCategory.Main),
        new Dish(15, "Chocolate Cake", 8.99m, DishCategory.Dessert),
        new Dish(16, "Cheesecake", 7.99m, DishCategory.Dessert),
        new Dish(17, "Apple Pie", 6.99m, DishCategory.Dessert),
        new Dish(18, "Tiramisu", 8.99m, DishCategory.Dessert),
        new Dish(19, "Ice Cream Sundae", 5.99m, DishCategory.Dessert),
        new Dish(20, "Crème Brûlée", 7.99m, DishCategory.Dessert)
    };

            // Generate 50 orders
            for (int i = 1; i <= 50; i++)
            {
                // Select random customer
                Customer randomCustomer = customers[random.Next(customers.Count)];

                // Create order items list
                List<OrderItem> orderItems = new List<OrderItem>();

                // Add 1-4 random items
                int itemCount = random.Next(1, 5);
                for (int j = 0; j < itemCount; j++)
                {
                    Dish randomDish = sampleDishes[random.Next(sampleDishes.Count)];
                    int quantity = random.Next(1, 4);

                    OrderItem orderItem = new OrderItem(
                        randomDish.Id,
                        randomDish.Name,
                        randomDish.Price,
                        quantity
                    );
                    orderItems.Add(orderItem);
                }

                // Create order with customer and items
                Order order = new Order(randomCustomer, orderItems);

                // Set random date within last 30 days
                int daysAgo = random.Next(0, 30);
                int hoursAgo = random.Next(0, 24);
                int minutesAgo = random.Next(0, 60);

                // Create date in correct format
                order.OrderDate = DateTime.Now
                    .AddDays(-daysAgo)
                    .AddHours(-hoursAgo)
                    .AddMinutes(-minutesAgo);

                // Set status based on date
                if (daysAgo > 7) // Older orders
                {
                    order.Status = OrderStatus.Completed;
                }
                else if (daysAgo == 0) // Today's orders
                {
                    int statusRandom = random.Next(100);
                    if (statusRandom < 70)
                        order.Status = OrderStatus.Pending;
                    else if (statusRandom < 90)
                        order.Status = OrderStatus.Preparing;
                    else
                        order.Status = OrderStatus.Completed;
                }
                else // Recent orders
                {
                    int statusRandom = random.Next(100);
                    if (statusRandom < 10)
                        order.Status = OrderStatus.Pending;
                    else if (statusRandom < 30)
                        order.Status = OrderStatus.Preparing;
                    else if (statusRandom < 90)
                        order.Status = OrderStatus.Completed;
                    else
                        order.Status = OrderStatus.Cancelled;
                }

                orders.Add(order);
            }

            return orders;
        }
        #endregion
        #region Save and Load Data
        public void SaveOrdersToCsv(List<Order> orders)
        {

            // Save Orders
            using (StreamWriter writer = new StreamWriter(_ordersFilePath))
            {
                // Write header
                writer.WriteLine("OrderId,CustomerId,CustomerName,OrderDate,Status,Total");

                // Write each order
                foreach (Order order in orders)
                {
                    writer.WriteLine($"{order.OrderId},{order.Customer.UserId},{order.Customer.Name}," +
                                   $"{order.OrderDate},{order.Status},{order.Total}");
                }
            }

            // Save Order Items
            using (StreamWriter writer = new StreamWriter(_itemsFilePath))
            {
                // Write header
                writer.WriteLine("OrderId,DishId,DishName,Price,Quantity");

                // Write items for each order
                foreach (Order order in orders)
                {
                    foreach (OrderItem item in order.Items)
                    {
                        writer.WriteLine($"{order.OrderId},{item.DishId},{item.DishName},{item.Price},{item.Quantity}");
                    }
                }
            }
        }
        public void SaveOrdersToCsv()
        {

            // Save Orders
            using (StreamWriter writer = new StreamWriter(_ordersFilePath))
            {
                // Write orders header
                writer.WriteLine("OrderId,CustomerId,CustomerName,OrderDate,Status,Total");

                // Write each order
                foreach (Order order in _orders)
                {
                    writer.WriteLine($"{order.OrderId}," +
                                    $"{order.Customer.UserId}," +
                                    $"{order.Customer.Name}," +
                                    $"{order.OrderDate}," +
                                    $"{order.Status}," +
                                    $"{order.Total}");
                }
            }

            // Save Order Items
            using (StreamWriter writer = new StreamWriter(_itemsFilePath))
            {
                // Write order items header
                writer.WriteLine("OrderId,DishId,DishName,Price,Quantity,SubTotal");

                // Write items for each order
                foreach (Order order in _orders)
                {
                    foreach (OrderItem item in order.Items)
                    {
                        writer.WriteLine($"{order.OrderId}," +
                                        $"{item.DishId}," +
                                        $"{item.DishName}," +
                                        $"{item.Price}," +
                                        $"{item.Quantity}," +
                                        $"{item.SubTotal}");
                    }
                }
            }
        }
        public List<Order> LoadOrdersFromCsv()
        {
            if (!File.Exists(_ordersFilePath) || !File.Exists(_itemsFilePath))
            {
                // If files don't exist, generate sample data
                List<Order> sampleOrders = GenerateSampleOrders();
                SaveOrdersToCsv(sampleOrders);
                return sampleOrders;
            }

            // Load customers first
            List<Customer> customers = LoadCustomersFromCsv();

            // Read all order items first into a dictionary
            Dictionary<int, List<OrderItem>> orderItems = new Dictionary<int, List<OrderItem>>();

            using (StreamReader reader = new StreamReader(_itemsFilePath))
            {
                // Skip header
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split(',');
                    int orderId = int.Parse(values[0]);
                    OrderItem item = new OrderItem(
                        int.Parse(values[1]),     // DishId
                        values[2],                // DishName
                        decimal.Parse(values[3]),  // Price
                        int.Parse(values[4])       // Quantity
                    );
                    // if order is not here, then create a list of order item for that order
                    if (!orderItems.ContainsKey(orderId))
                    {
                        orderItems[orderId] = new List<OrderItem>();
                    }
                    orderItems[orderId].Add(item);
                }
            }

            // read orders
            List<Order> loadedOrders = new List<Order>();
            string[] dateFormats = new string[]
            {
                "yyyy-MM-dd h:mm:ss tt",  // format are using
                "yyyy-MM-dd hh:mm:ss tt", 
                "yyyy-MM-dd H:mm:ss"     
            };
            using (StreamReader reader = new StreamReader(_ordersFilePath))
            {
                // Skip header
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    try
                    {
                        string[] values = reader.ReadLine().Split(',');
                        int orderId = int.Parse(values[0]);
                        string customerName = values[2];
                        Customer customer = customers.Find(c => c.Name == customerName);
                        if (customer == null) 
                        {
                            customer = new Customer(customerName);
                        }

                        Order order = new Order(customer, orderItems[orderId]);
                        order.OrderId = orderId;

                        // Try parsing with our new formats
                        if (DateTime.TryParseExact(values[3], dateFormats,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime orderDate))
                        {
                            order.OrderDate = orderDate;
                        }
                        else
                        {
                            //MessageBox.Show($"Invalid date format in order {orderId}: {values[3]}");
                            Debug.WriteLine($"Invalid date format in order {orderId}: {values[3]}");
                            continue;
                        }

                        order.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), values[4]);
                        loadedOrders.Add(order);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading order: {ex.Message}");
                        continue;
                    }
                }
            }

            return loadedOrders;
        }
        public static void SaveCustomersToCsv(List<Customer> customers)
        {
            using (StreamWriter writer = new StreamWriter(_customersFilePath))
            {
                // Write header
                writer.WriteLine("Id,Name,Email,Phone,TotalSpent");

                // Write each customer\\\\\
                foreach (Customer customer in customers)
                {
                    writer.WriteLine($"{customer.UserId},{customer.Name},{customer.Email},{customer.Phone},{customer.GetTotalSpent()}");
                }
            }
        }
        public static List<Customer> LoadCustomersFromCsv()
        {
            if (!File.Exists(_customersFilePath))
            {
                // If file doesn't exist, generate sample customers and save them
                List<Customer> customers = GenerateSampleCustomers();
                SaveCustomersToCsv(customers);
                return customers;
            }

            List<Customer> loadedCustomers = new List<Customer>();
            using (StreamReader reader = new StreamReader(_customersFilePath))
            {
                // Skip header
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split(',');
                    Customer customer = new Customer(
                        name: values[1],
                        email: values[2],
                        phone: values[3]
                    );
                    loadedCustomers.Add(customer);
                }
            }

            return loadedCustomers;
        }
        private void SaveDishesToCsv(List<Dish> dishes)
        {
            using (StreamWriter writer = new StreamWriter(_csvDishesFilePath))
            {
                // Write header
                writer.WriteLine("Id,Name,Price,Category,IsAvailable,ImagePath");

                // Write dishes
                foreach (Dish dish in dishes)
                {
                    writer.WriteLine($"{dish.Id},{dish.Name},{dish.Price},{dish.Category},{dish.IsAvailable},{dish.ImagePath}");
                }
            }
        }
        private List<Dish> LoadDishesFromCsv()
        {
            try
            {
                if (!File.Exists(_csvDishesFilePath))
                {
                    var dishes = GenerateRestaurantDishes();
                    SaveDishesToCsv(dishes);
                    return dishes;
                }

                List<Dish> loadedDishes = new List<Dish>();
                using (StreamReader reader = new StreamReader(_csvDishesFilePath))
                {
                    // Skip header
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string[] values = reader.ReadLine().Split(',');
                        loadedDishes.Add(new Dish(
                            int.Parse(values[0]),     // Id
                            values[1],                // Name
                            decimal.Parse(values[2]),  // Price
                            (DishCategory)Enum.Parse(typeof(DishCategory), values[3]), // Category
                            values[5]                 // ImagePath
                        ));
                    }
                }
                return loadedDishes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating/loading dishes file: {ex.Message}");
                return new List<Dish>();
            }
        }
        #endregion
    }
}
