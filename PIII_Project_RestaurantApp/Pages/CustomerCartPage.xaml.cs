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
using PIII_Project_RestaurantApp.Views;

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
            // Clear original data source
            CartItemsListView.ItemsSource = null;
            // Set up new data source
            CartItemsListView.ItemsSource = items;
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

                var confirmWindow = new OrderConfirmationWindow(_currentCustomer, _currentCustomer.GetCartTotal());
                if (confirmWindow.ShowDialog() == true)
                {
                    MessageBox.Show("Order placed successfully!");
                    NavigationService?.Navigate(new CustomerOrderPage(_currentCustomer, confirmWindow.CreatedOrder));
                    _currentCustomer.ClearCart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
