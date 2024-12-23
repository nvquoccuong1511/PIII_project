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
using System.Windows.Shapes;
using PIII_Project_RestaurantApp.Models;
using PIII_Project_RestaurantApp.Pages;

namespace PIII_Project_RestaurantApp.Views
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private Customer _currentCustomer;
        public CustomerWindow(Customer customer)
        {
            InitializeComponent();
            _currentCustomer = customer;
        }

        private void btnStarterMenu_Clicked(object sender, RoutedEventArgs e)
        {
            MenuContent.Navigate(new StarterMenuPage(_currentCustomer));
        }

        private void btnMainMenu_Clicked(object sender, RoutedEventArgs e)
        {
            MenuContent.Navigate(new MainMenuPage(_currentCustomer));
        }

        private void btnDessertMenu_Clicked(object sender, RoutedEventArgs e)
        {
            MenuContent.Navigate(new DessertMenuPage(_currentCustomer));
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            MenuContent.Navigate(new CustomerCartPage(_currentCustomer));
        }

        private void btnReviewOrder_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Order button clicked");
            Debug.WriteLine($"Current customer orders: {_currentCustomer.OrderHistory.Count}");
            MenuContent.Navigate(new CustomerOrderHistoryPage(_currentCustomer));
        }
        private void btnSignOut_Clicked(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to sign out?", "Confirm Sign Out", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Window.GetWindow(this).Close();
            }
        }
    }
}
