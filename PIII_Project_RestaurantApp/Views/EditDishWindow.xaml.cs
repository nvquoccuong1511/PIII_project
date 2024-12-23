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
using System.IO;

namespace PIII_Project_RestaurantApp.Views
{
    /// <summary>
    /// Interaction logic for EditDishWindow.xaml
    /// </summary>
    public partial class EditDishWindow : Window
    {
        private readonly Owner _owner;
        private Dish _dish;
        public EditDishWindow(Dish dish, Owner owner)
        {
            InitializeComponent();
            _dish = dish;
            LoadSelectedDish();
            LoadCategories();
            _owner = owner;
        }
        private void LoadSelectedDish()
        {
            // binding each property with corresponding dish data
            txtId.Text = _dish.Id.ToString();
            txtName.Text = _dish.Name;
            txtPrice.Text = _dish.Price.ToString();
            cmbCategory.SelectedItem = _dish.Category;
            chkAvailable.IsChecked = _dish.IsAvailable;

            // need to add image path later
            // Load image if path exists

            if (!string.IsNullOrEmpty(_dish.ImagePath))
            {
                try
                {
                    string imagePath = $@"..\..\..\Images\{_dish.ImagePath}";
                    //MessageBox.Show($"Trying to load from: {imagePath}");

                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(imagePath, UriKind.Relative);
                    image.EndInit();
                    imgDish.Source = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}");
                }
            }
        }
        private void LoadCategories()
        {
            // This will add all values from DishCategory enum to the ComboBox
            cmbCategory.ItemsSource = Enum.GetValues(typeof(DishCategory));

            // Select the current category of the dish
            cmbCategory.SelectedItem = _dish.Category;
        }
        //private void LoadDishImage()
        //{
        //    if (!string.IsNullOrEmpty(_dish.ImagePath))
        //    {
        //        try
        //        {
        //            // full path
        //            string imagePath = $@"..\..\..\Images\{_dish.ImagePath}";
        //            imgDish.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error loading image: {ex.Message}");
        //        }
        //    }
        //}
        private void btnChangeImage_Clicked(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    // Get file name
                    string fileName = dialog.FileName.Substring(dialog.FileName.LastIndexOf('\\') + 1);

                    // Save relative to category folder
                    string newRelativePath = $"{_dish.Category}/{fileName}";
                    string fullPath = $@"..\..\..\Images\{newRelativePath}";

                    // Copy the file
                    File.Copy(dialog.FileName, fullPath, true);

                    // Create a new BitmapImage
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;  // Important for refreshing
                    image.UriSource = new Uri(fullPath, UriKind.Relative);
                    image.EndInit();

                    // Update image in UI
                    imgDish.Source = image;

                    // Update dish with relative path
                    _owner.UpdateDishImage(newRelativePath, _dish);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling file: {ex.Message}");
            }
        }
        

        private void btnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnSave_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Name cannot be empty", "Validation Error");
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Please enter a valid price", "Validation Error");
                    return;
                }

                string name = txtName.Text;
                bool isAvailable = chkAvailable.IsChecked ?? false;
                DishCategory category = (DishCategory)cmbCategory.SelectedItem;
                string imagePath = _dish.ImagePath; // Use the existing image path

                _owner.UpdateDish(_dish.Id, name, price, isAvailable, category, imagePath);

                MessageBox.Show("Dish updated successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
