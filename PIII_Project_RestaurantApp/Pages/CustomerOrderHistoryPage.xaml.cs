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
using System.IO;


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
            string filePath = @"..\..\..\Data\orders.csv";
            if (!File.Exists(filePath))
            {
                Debug.WriteLine("Orders file not found");
                return;
            }

            var orders = new List<OrderDisplay>();
            var lines = File.ReadAllLines(filePath).Skip(1);

            foreach (var line in lines)
            {
                var fields = line.Split(',');
                if (fields[2] == _currentCustomer.Name.ToLower())
                {
                    orders.Add(new OrderDisplay
                    {
                        OrderId = int.Parse(fields[0]),
                        OrderDate = DateTime.Parse(fields[3]),
                        Status = fields[4],
                        Total = decimal.Parse(fields[5])
                    });
                }
            }
            OrdersListView.ItemsSource = orders;
        }

    }
}
