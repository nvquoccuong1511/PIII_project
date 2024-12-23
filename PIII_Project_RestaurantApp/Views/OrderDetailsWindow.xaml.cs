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

namespace PIII_Project_RestaurantApp.Views
{
    /// <summary>
    /// Interaction logic for OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        private readonly Order _order;

        public OrderDetailsWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            // Order information
            txtOrderId.Text = _order.OrderId.ToString();
            txtOrderDate.Text = _order.OrderDate.ToString("dd/MM/yyyy HH:mm");
            txtStatus.Text = _order.Status.ToString();

            // Customer information
            if (_order.Customer != null)
            {
                txtCustomerName.Text = _order.Customer.Name;
                txtCustomerEmail.Text = _order.Customer.Email;
                txtCustomerPhone.Text = _order.Customer.Phone;
            }

            // Order items
            orderItemsGrid.ItemsSource = _order.Items;

            // Total
            txtTotal.Text = _order.Total.ToString("C");
        }
    }
}
