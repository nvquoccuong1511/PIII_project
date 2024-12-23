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
    /// Interaction logic for OwnerAuthWindow.xaml
    /// </summary>
    public partial class OwnerAuthWindow : Window
    {
        private Owner _owner;
        private string _userName;
        public OwnerAuthWindow(string name)
        {
            _userName = name;
            InitializeComponent();
        }

        private void btnLogin_Clicked(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            if (IsValidateLogin(email, password))
            {
                _owner = new Owner(_userName, email);
                OwnerWinDow theBossWindow = new OwnerWinDow(_owner);
                theBossWindow.Show();
                this.Close();
            }
        }

        private void btnCancel_Clicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();

        }

        private bool IsValidateLogin(string email, string password)
        {
            // Check if email is empty
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email cannot be empty", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            // Check if password is empty
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password cannot be empty", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return false;
            }

            // Validate email format
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Invalid email format. Please enter a valid email address",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            // Check credentials, later will implement a place to save owner password
            if (email != "cmders@restaurant.com" || password != "cmders999")
            {
                MessageBox.Show("Invalid email or password", "Authentication Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtPassword.Clear();
                txtEmail.Focus();
                return false;

            }

            return true;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                passwordPlaceholder.Visibility = Visibility.Collapsed;
            };
        }

        private void Log_In(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Clicked(sender, e);
            }
        }

    }
}
