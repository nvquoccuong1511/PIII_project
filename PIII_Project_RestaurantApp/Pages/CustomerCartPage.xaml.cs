using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PIII_Project_RestaurantApp.Models;
using System.IO;

namespace PIII_Project_RestaurantApp.Pages
{
    /// <summary>
    /// Interaction logic for CustomerCartPage.xaml
    /// </summary>
    public partial class CustomerCartPage : Page
    {
        private Customer _currentCustomer;
        public CustomerCartPage(Customer customer)
        {
            InitializeComponent();
            _currentCustomer = customer;
            LoadCartItems();
        }
        private void LoadCartItems()
        {
            var items = _currentCustomer.GetCartItems();
            CartItemsListView.ItemsSource = null;  // Clear original data source
            CartItemsListView.ItemsSource = items; // Set up new data source
            UpdateTotal();
        }
        private void UpdateTotal()
        {
            decimal total = _currentCustomer.GetCartTotal();
            TotalText.Text = $"Total: ${total:F2}";
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (OrderItem)button.DataContext;
            if (item.Quantity > 1)
            {
                item.Quantity--;
                CartItemsListView.Items.Refresh();
                UpdateTotal();
            }
        }
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (OrderItem)button.DataContext;
            item.Quantity++;
            CartItemsListView.Items.Refresh();
            UpdateTotal();
        }

        private void RemoveItem_Clicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (OrderItem)button.DataContext;
            _currentCustomer.RemoveFromCart(item);
            LoadCartItems();
        }
        private void PlaceOrder_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentCustomer.GetCartItems().Count == 0)
                {
                    MessageBox.Show("Cart is empty!");
                    return;
                }

                var order = new Order(_currentCustomer,_currentCustomer.GetCartItems());
                // save order in CSV file
                SaveOrderToCSV(order); 

                MessageBox.Show("Order placed successfully!");
                // Navigation to order Page
                NavigationService?.Navigate(new CustomerOrderPage(_currentCustomer, order));
                _currentCustomer.ClearCart();
                LoadCartItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error placing order: {ex.Message}");
            }
        }
        private void SaveOrderToCSV(Order order)
        {
            try
            {
                string filePath = @"..\..\..\Data\orders.csv";
                string directoryPath = System.IO.Path.GetDirectoryName(filePath);

                // check file path
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // check file permission
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                }

                // Gets an existing order ID
                List<int> existingIds = new List<int>();
                if (File.Exists(filePath))
                {
                    // Skip header line
                    var lines = File.ReadAllLines(filePath).Skip(1); 
                    foreach (var line in lines)
                    {
                        var id = int.Parse(line.Split(',')[0]);
                        existingIds.Add(id);
                    }
                }

                // Set the ID of the new order
                order.OrderId = existingIds.Count > 0 ? existingIds.Max() + 1 : 1;

                // Prepare new order data
                string orderLine = $"{order.OrderId},{order.Status},{order.OrderDate:yyyy-MM-dd h:mm:ss tt},{order.Total:F2}";

                // 如果文件不存在，If the file does not exist, write the header line first
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "OrderId,Status,OrderDate,Total\n");
                }

                // Additional new order
                File.AppendAllText(filePath, orderLine + "\n");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("No permission to write to file. Please check file permissions.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving order to CSV: {ex.Message}");
            }
        }
    }
}
