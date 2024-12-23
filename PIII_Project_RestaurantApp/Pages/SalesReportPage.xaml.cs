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

namespace PIII_Project_RestaurantApp.Pages
{
    public class DailySales
    {
        public DateTime Date { get; set; }
        public decimal Sales { get; set; }
        public int Orders { get; set; }
        public decimal Average { get { return Orders > 0 ? Sales / Orders : 0; } }
    }
    /// <summary>
    /// Interaction logic for SalesReportPage.xaml
    /// </summary>
    public partial class SalesReportPage : Page
    {
        private readonly Owner _owner;
        private List<DailySales> _salesReport;
        public SalesReportPage(Owner owner)
        {
            InitializeComponent();
            _owner = owner;
        }
        private void InitializeDatePickers()
        {
            // Set default date range (last 7 days)
            dateStart.SelectedDate = DateTime.Today.AddDays(-7);
            dateEnd.SelectedDate = DateTime.Today;
        }

        private void btnGenerateReport_Clicked(object sender, RoutedEventArgs e)
        {
            if (!dateStart.SelectedDate.HasValue || !dateEnd.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select both start and end dates", "Missing Dates",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime startDate = dateStart.SelectedDate.Value;
            DateTime endDate = dateEnd.SelectedDate.Value;

            if (endDate < startDate)
            {
                MessageBox.Show("End date must be after start date", "Invalid Date Range",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GenerateReport(startDate, endDate);
        }
        private decimal CalculateDailySales(List<Order> dailyOrders)
        {
            decimal totalSales = 0;

            // If no orders for this day, return 0
            if (dailyOrders == null || dailyOrders.Count == 0)
            {
                return totalSales;
            }

            //  each order
            for (int i = 0; i < dailyOrders.Count; i++)
            {
                Order currentOrder = dailyOrders[i];

                // Skip cancelled orders
                if (currentOrder.Status == OrderStatus.Cancelled)
                {
                    continue;
                }

                // Add this order's total to the daily sales
                totalSales += currentOrder.Total;
            }

            return totalSales;
        }
        private void GenerateReport(DateTime startDate, DateTime endDate)
        {
            _salesReport = new List<DailySales>();
            decimal totalSales = 0;
            int totalOrders = 0;

            // Generate report for each day in the range
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Get orders for this day
                List<Order> dailyOrders = _owner.GetDailyOrders(date);

                // Calculate sales for this day
                decimal dailySales = CalculateDailySales(dailyOrders);

                // Count valid orders (not cancelled)
                int validOrderCount = 0;
                for (int i = 0; i < dailyOrders.Count; i++)
                {
                    if (dailyOrders[i].Status != OrderStatus.Cancelled)
                    {
                        validOrderCount++;
                    }
                }

                // Create sales record for this day
                DailySales dailySalesRecord = new DailySales
                {
                    Date = date,
                    Sales = dailySales,
                    Orders = validOrderCount
                };

                // Add to report
                _salesReport.Add(dailySalesRecord);

                // Update totals
                totalSales += dailySales;
                totalOrders += validOrderCount;
            }

            // Update to UI
            salesGrid.ItemsSource = _salesReport;
            txtTotalSales.Text = totalSales.ToString("C");
            txtTotalOrders.Text = totalOrders.ToString();

            // Calculate overall average order value
            decimal averageOrderValue = 0;
            if (totalOrders > 0)
            {
                averageOrderValue = totalSales / totalOrders;
            }
            txtAverageOrder.Text = averageOrderValue.ToString("C");
        }
    }
}
