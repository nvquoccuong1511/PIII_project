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
using System.Windows.Shapes;
using PIII_Project_RestaurantApp.Models;
using PIII_Project_RestaurantApp.Pages;


namespace PIII_Project_RestaurantApp.Views
{

    /// <summary>
    /// Interaction logic for OwnerWinDow.xaml
    /// </summary>
    /// 

    public partial class OwnerWinDow : Window
    {
        private Owner _owner;
        public OwnerWinDow(Owner owner)
        {
            _owner = owner;
            InitializeComponent();
        }

        private void btnMenuManagement_Clicked(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new MenuManagementPage(_owner));
        }

        private void btnOrderManagement_Clicked(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new OrderManagement(_owner));
        }

        private void btnSalesReport_Clicked(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new SalesReportPage(_owner));
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
