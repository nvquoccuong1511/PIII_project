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
    /// Interaction logic for AddNewDishWindow.xaml
    /// </summary>
    public partial class AddNewDishWindow : Window
    {
        private Owner _owner;
        private string _imagePath = "";


        public AddNewDishWindow(Owner owner)
        {
            InitializeComponent();
            cmbCategory.SelectedItem = true;// default
            _owner = owner;
            LoadCategories();
        }
        private void LoadCategories()
        {
            cmbCategory.ItemsSource = Enum.GetValues(typeof(DishCategory));
            cmbCategory.SelectedIndex = 0; // Select first category as a default
        }
        private void btnAddImage_Clicked(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (dialog.ShowDialog() == true)
            {
                string fileName = System.IO.Path.GetFileName(dialog.FileName);
                string destinationPath = System.IO.Path.Combine(@"..\..\..\Images", fileName);

                try
                {
                    System.IO.File.Copy(dialog.FileName, destinationPath, true);
                    _imagePath = fileName;

                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(destinationPath, UriKind.Relative);
                    image.EndInit();

                    imgDish.Source = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding image: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// Get the valid id to set for the new dish.
        /// </summary>
        /// <returns></returns>
        private int GetNewDishId()
        {
            List<Dish> curDishes = _owner.GetDishes();
            if (curDishes != null && curDishes.Count == 0)
            {
                return 1; // id start from 1 if current list is empty.
            }
            int newId = 0;
            foreach (Dish dish in curDishes)
            {
                if (dish.Id > newId)
                {
                    newId = dish.Id;
                }
            }
            newId++;
            return newId;
        }

        private void btnAdd_Clicked(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a dish name", "Validation Error");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price", "Validation Error");
                return;
            }
            try
            {
                int newId = GetNewDishId();
                // new dish
                Dish newDish = new Dish(newId, txtName.Text, price, (DishCategory)cmbCategory.SelectedItem, _imagePath);
                _owner.AddDish(newDish);

                MessageBox.Show("Add new dish successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Set dialog result
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding dish: {ex.Message}", "Error");
            }
        }

        private void btnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
