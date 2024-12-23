using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PIII_Project_RestaurantApp.Pages
{
    /// <summary>
    /// Interaction logic for CustomerOrderPage.xaml
    /// </summary>
    public partial class CustomerOrderPage : Page
    {
        private Customer _currentCustomer;
        private Order _currentOrder;
        public CustomerOrderPage(Customer customer, Order order)
        {
            InitializeComponent();
            _currentCustomer = customer;
            _currentOrder = order;
            _currentCustomer.OrderHistory.Add(order);
            LoadOrderDetails();
        }
        private void LoadOrderDetails()
        {
            try
            {
                // display order info
                OrderIdText.Text = _currentOrder.OrderId.ToString();
                OrderDateText.Text = _currentOrder.OrderDate.ToString("MM/dd/yyyy HH:mm");
                StatusText.Text = _currentOrder.Status.ToString();

                foreach (var item in _currentOrder.Items)
                {
                    Debug.WriteLine($"Item: {item.DishName}, Quantity: {item.Quantity}, Price: {item.Price}");
                }

                // display order item 
                OrderItemsListView.ItemsSource = null; // clear 
                OrderItemsListView.ItemsSource = _currentOrder.Items;

                // Get total spend
                decimal total = _currentOrder.Total;
                TotalText.Text = $"Total: ${total:F2}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoadOrderDetails: {ex.Message}");
                MessageBox.Show($"Error loading order details: {ex.Message}");
            }
            UpdateTotal();
        }
        private void UpdateTotal()
        {
            decimal total = _currentOrder.Total;
            TotalText.Text = $"Total: ${total:F2}";
        }
    }
}
