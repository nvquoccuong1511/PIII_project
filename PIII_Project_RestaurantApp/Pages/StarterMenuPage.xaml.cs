using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for StarterMenuPage.xaml
    /// </summary>
    public partial class StarterMenuPage : Page
    {
        private List<Dish> _desserts;
        private Customer _currentCustomer;
        public StarterMenuPage(Customer customer)
        {
            InitializeComponent();
            _currentCustomer = customer;
            _desserts = new List<Dish>();
            LoadDishes();
            DisplayDesserts();
        }
        private void LoadDishes()
        {
            try
            {
                // Check File.Exists 
                Console.WriteLine($"File exists: {File.Exists(_currentCustomer.CsvFilePath)}");

                var allDishes = _currentCustomer.GetAllDishes();
                Console.WriteLine($"Total dishes loaded: {allDishes.Count}");

                foreach (Dish dish in allDishes)
                {
                    if (dish.Category == DishCategory.Starter)
                    {
                        _desserts.Add(dish);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void DisplayDesserts()
        {
            try
            {
                Debug.WriteLine($"Found {_desserts.Count} desserts");
                for (int i = 0; i < _desserts.Count && i < 6; i++)
                {
                    var dish = _desserts[i];
                    var row = i / 3;
                    var col = i % 3;

                    var stackPanel = GetStackPanelAt(row, col);
                    if (stackPanel != null)
                    {
                        var image = stackPanel.Children[0] as Image;
                        if (image != null)
                        {
                            try
                            {
                                var imagePath = $"/Images/{dish.ImagePath}";
                                Debug.WriteLine($"Loading image from: {imagePath}");
                                image.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error loading image for {dish.Name}: {ex.Message}");
                            }
                        }
                        var nameText = stackPanel.Children[1] as TextBlock;
                        if (nameText != null)
                        {
                            nameText.Text = dish.Name;
                        }

                        var priceText = stackPanel.Children[2] as TextBlock;
                        if (priceText != null)
                        {
                            priceText.Text = $"${dish.Price:F2}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in DisplayDesserts: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private StackPanel GetStackPanelAt(int row, int col)
        {
            foreach (var child in DishesGrid.Children)
            {
                if (child is StackPanel panel &&
                    Grid.GetRow(panel) == row &&
                    Grid.GetColumn(panel) == col)
                {
                    return panel;
                }
            }
            return null;
        }

        private void AddToCartBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var stackPanel = (StackPanel)button.Parent;
            var nameText = (TextBlock)stackPanel.Children[1];
            var priceText = (TextBlock)stackPanel.Children[2];

            // Gets the currently selected menu
            Dish selectedDish = null;
            foreach (var dessert in _desserts)
            {
                if (dessert.Name == nameText.Text)
                {
                    selectedDish = dessert;
                    break;
                }
            }
            if (selectedDish != null)
            {
                _currentCustomer.AddToCart(selectedDish);
                MessageBox.Show($"Added {selectedDish.Name} (${selectedDish.Price:F2}) to cart");
            }
        }
    }
}
