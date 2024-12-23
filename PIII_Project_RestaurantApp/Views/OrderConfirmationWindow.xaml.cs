using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PIII_Project_RestaurantApp.Models;

namespace PIII_Project_RestaurantApp.Views
{
    /// <summary>
    /// Interaction logic for OrderConfirmationWindow.xaml
    /// </summary>
    public partial class OrderConfirmationWindow : Window
    {
        private Customer _currentCustomer;
        public Order CreatedOrder { get; private set; }
        public OrderConfirmationWindow(Customer customer, decimal total)
        {
            InitializeComponent();
            _currentCustomer = customer;
        }
        private void BtnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnConfirm_Clicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerPhone.Text))
            {
                MessageBox.Show("Please enter both name and phone number.");
                return;
            }
            // validate for phone
            if (!ValidatePhoneNumber(txtCustomerPhone.Text))
            {
                MessageBox.Show("Please enter a valid phone number (digits only, 8-15 digits)");
                return;
            }

            try
            {
                // update customer info
                _currentCustomer.Name = txtCustomerName.Text.Trim().ToLower();
                _currentCustomer.Phone = txtCustomerPhone.Text.Trim();

                // create order
                var order = new Order(_currentCustomer, _currentCustomer.GetCartItems());
                SaveOrderToCSV(order);
                CreatedOrder = order;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error placing order: {ex.Message}");
            }
        }
        private bool ValidatePhoneNumber(string phone)
        {
            phone = phone.Trim().Replace(" ", "");

            // check length
            if (phone.Length < 8 || phone.Length > 15)
            {
                return false;
            }
            // check all is number
            foreach (char c in phone)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private int GenerateOrderId()
        {
            string filePath = @"..\..\..\Data\orders.csv";

            // Get existing order IDs
            List<int> existingIds = new List<int>();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).Skip(1);
                foreach (var line in lines)
                {
                    if (int.TryParse(line.Split(',')[0], out int id))
                    {
                        existingIds.Add(id);
                    }
                }
            }

            // Set the new order ID 
            return existingIds.Count + 1;
        }

        private void SaveOrderToCSV(Order order)
        {
            try
            {
                // Paths for orders and order items CSV files
                string ordersFilePath = @"..\..\..\Data\orders.csv";
                string orderItemsFilePath = @"..\..\..\Data\order_items.csv";

                // Ensure directory exists
                string directoryPath = System.IO.Path.GetDirectoryName(ordersFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Generate the new order ID
                order.OrderId = GenerateOrderId();

                // Save Order to orders.csv
                string orderLine = $"{order.OrderId},{_currentCustomer.UserId}," +
                    $"{_currentCustomer.Name},{order.OrderDate:yyyy-MM-dd HH:mm:ss}," +
                    $"{order.Status},{order.Total:F2}";

                // Write the header if the file doesn't exist
                if (!File.Exists(ordersFilePath))
                {
                    File.WriteAllText(ordersFilePath, "OrderId,CustomerId,CustomerName,OrderDate,Status,Total\n");
                }

                // Append the new order line to the orders file
                File.AppendAllText(ordersFilePath, orderLine + "\n");

                // Save Order Items to order_items.csv
                // Write the header if the file doesn't exist
                if (!File.Exists(orderItemsFilePath))
                {
                    File.WriteAllText(orderItemsFilePath, "OrderId,DishId,DishName,Price,Quantity\n");
                }

                // Prepare and save order items
                List<string> orderItemLines = new List<string>();
                foreach (var item in order.Items)
                {
                    string itemLine = $"{order.OrderId},{item.DishId},{item.DishName},{item.Price},{item.Quantity}";
                    orderItemLines.Add(itemLine);
                }

                // Append order items to the file
                File.AppendAllLines(orderItemsFilePath, orderItemLines);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving order: {ex.Message}");
            }
        }
    }
}
