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
    /// Interaction logic for CustomerOrderHistoryPage.xaml
    /// </summary>
    public partial class CustomerOrderHistoryPage : Page
    {
        private Customer _currentCustomer;

        public CustomerOrderHistoryPage(Customer customer)
        {
            InitializeComponent();
            _currentCustomer = customer;
            LoadOrderHistory();
        }

        private void LoadOrderHistory()
        {
            try
            {
                Debug.WriteLine($"Loading orders, count: {_currentCustomer.OrderHistory.Count}");
                OrdersListView.ItemsSource = _currentCustomer.OrderHistory;
                foreach (var order in _currentCustomer.OrderHistory)
                {
                    Debug.WriteLine($"OrderId: {order.OrderId}, OrderDate: {order.OrderDate}, Status: {order.Status}, TotalPrice: {order.Total}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading orders: {ex.Message}");
            }
        }
    }
}
