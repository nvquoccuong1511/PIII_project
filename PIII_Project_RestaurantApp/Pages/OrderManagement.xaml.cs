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
using PIII_Project_RestaurantApp.Views;

namespace PIII_Project_RestaurantApp.Pages
{
    /// <summary>
    /// Interaction logic for OrderManagement.xaml
    /// </summary>
    public partial class OrderManagement : Page
    {
        private Owner _owner;
        private List<Order> _orders;
        public OrderManagement(Owner owner)
        {
            InitializeComponent();
            _owner = owner;
            SetUp();
        }
        public void SetUp()
        {
            // today is the default
            datePicker.SelectedDate = DateTime.Today;
            cmbStatus.SelectedIndex = 0;
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add(new ComboBoxItem { Content = "All Orders" });

            // Add each enum value to ComboBox
            foreach (OrderStatus status in Enum.GetValues(typeof(OrderStatus)))
            {
                cmbStatus.Items.Add(new ComboBoxItem { Content = status.ToString() });
            }

            // Select "All Orders" by default
            cmbStatus.SelectedIndex = 0;

            // Load initial orders
            LoadOrders();
        }
        public void LoadOrders()
        {
            // if date is not selected, today is the default selected date
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Today;
            ComboBoxItem selectedItem = cmbStatus.SelectedItem as ComboBoxItem;
            string selectedStatus;
            if (selectedItem != null)
            {
                selectedStatus = selectedItem.Content.ToString();
                // Get orders for selected date
                _orders = _owner.GetDailyOrders(selectedDate);
                if (selectedStatus != "All Orders" && _orders != null)
                {
                    List<Order> fileredOrders = new List<Order>();
                    // convert string status to enum
                    OrderStatus status = (OrderStatus)Enum.Parse(typeof(OrderStatus), selectedStatus);
                    // Add orders of the selected status to the filteredOrder list
                    foreach (Order order in _orders)
                    {
                        if (order.Status == status)
                        {
                            fileredOrders.Add(order);
                        }
                    }
                    _orders = fileredOrders;
                }
            }
            ordersGrid.ItemsSource = _orders;
            ordersGrid.Items.Refresh();
        }
        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if component is fully loaded
            if (!IsLoaded) return;

            // Get selected date
            DateTime selectedDate;

            if (datePicker.SelectedDate.HasValue)
            {
                selectedDate = datePicker.SelectedDate.Value;

                // Optional: Check if selected date is in future
                if (selectedDate.Date > DateTime.Today)
                {
                    MessageBox.Show("Cannot select future date", "Invalid Date",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    datePicker.SelectedDate = DateTime.Today;
                    return;
                }
                datePicker.SelectedDate = selectedDate;
            }
            else
            {
                // If no date selected, use today
                datePicker.SelectedDate = DateTime.Today;
            }

            // Reload orders for selected date
            LoadOrders();
        }
        private void btnToday_Clicked(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = DateTime.Today;
            LoadOrders();
        }

        private void btnRefresh_Clicked(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void btnPreviousDay_Clicked(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate.HasValue)
            {
                datePicker.SelectedDate = datePicker.SelectedDate.Value.AddDays(-1);
            }

        }

        private void btnNextDay_Clicked(object sender, RoutedEventArgs e)
        {
            DateTime nextDay = datePicker.SelectedDate?.AddDays(1) ?? DateTime.Today;

            // Don't allow selecting future dates
            if (nextDay.Date <= DateTime.Today)
            {
                datePicker.SelectedDate = nextDay;
            }
        }

        private void cmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Call LoadOrders to refresh the grid with the new filter
            LoadOrders();
        }

        private void ordersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Order selectedOrder = ordersGrid.SelectedItem as Order;
            if (selectedOrder != null)
            {
                OrderDetailsWindow detailsWindow = new OrderDetailsWindow(selectedOrder);
                detailsWindow.ShowDialog();
            }
        }
    }
}
