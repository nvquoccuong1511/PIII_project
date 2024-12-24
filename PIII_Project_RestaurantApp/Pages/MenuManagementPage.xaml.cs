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
//using PIII_Project_RestaurantApp.Models;

namespace PIII_Project_RestaurantApp.Pages
{
    /// <summary>
    /// Interaction logic for MenuManagementPage.xaml
    /// </summary>
    public partial class MenuManagementPage : Page
    {
        private List<Dish> _dishes;
        private readonly Owner _owner = new Owner("Cuong", "");

        public MenuManagementPage(Owner owner)
        {
            InitializeComponent();
            _owner = owner;
            LoadDishes();
        }
        private void LoadDishes()
        {
            // retrieve dishes from owner
            dishesGrid.ItemsSource = _owner.Dishes;
            dishesGrid.Items.Refresh();
        }

        private void btnRemoveDish_Clicked(object sender, RoutedEventArgs e)
        {
            Dish selectedDish = dishesGrid.SelectedItem as Dish;

            if (selectedDish == null)
            {
                MessageBox.Show("Please select a dish to remove", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to remove {selectedDish.Name}?",
                "Confirm Removal",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _owner.RemoveDish(selectedDish);
                LoadDishes();

            }
        }

        private void btnAddDish_Clicked(object sender, RoutedEventArgs e)
        {
            
            AddNewDishWindow addNewDishWindow = new AddNewDishWindow(_owner);
            if (addNewDishWindow.ShowDialog() == true)
            {

                LoadDishes();
            }
            
        }

        private void btnEditDish_Clicked(object sender, RoutedEventArgs e)
        {
            Dish selectedDish = dishesGrid.SelectedItem as Dish;

            if (selectedDish == null)
            {
                MessageBox.Show("Please select a dish to edit", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            EditDishWindow editWindow = new EditDishWindow(selectedDish, _owner);
            if (editWindow.ShowDialog() == true)
            {
                // Refresh the DataGrid if changes were made
                LoadDishes();
            }
        }
    }
}
