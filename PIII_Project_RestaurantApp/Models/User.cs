using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIII_Project_RestaurantApp.Models
{
    public abstract class User
    {
        private int _userId;
        private string _name;
        private string _email;
        private string _password;

        protected User(string name)
        {
            Name = name;
        }
        protected User(string name, string email) : this(name) 
        {
            Email = email;
        }
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            private set { _password = value; }

        }
        public virtual bool ValidateCredentials(string password)
        {
            return password==Password;
        }
        public virtual void UpdateProfile(string name, string email)
        {
            _name = name;
            _email = email;
        }
        public string GetUserDetails()
        {
            return $"Name: {Name}, Email: {Email}";
        }
    }
}
