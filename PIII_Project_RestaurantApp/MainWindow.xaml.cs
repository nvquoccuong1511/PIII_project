using System.Text;
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
using PIII_Project_RestaurantApp.Views;

namespace PIII_Project_RestaurantApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOwner_Clicked(object sender, RoutedEventArgs e)
        {
            // Create owner and navigate to owner window
            //Owner owner = new Owner(txtUsername.Text, "");
            OwnerAuthWindow ownerWindow = new OwnerAuthWindow(txtUsername.Name);
            ownerWindow.Show();
            this.Hide();
        }

        private void btnCustomer_Clicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter your name", "Input Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create customer and navigate to customer window
            Customer customer = new Customer(txtUsername.Text, "");
            CustomerWindow customerWindow = new CustomerWindow(customer);
            customerWindow.Show();
            this.Close();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnCustomer_Clicked(sender, e);
            }
        }
    }
}