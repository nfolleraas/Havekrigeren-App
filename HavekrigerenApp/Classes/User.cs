using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Classes
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public User()
        {
        }

        public static bool Login(string username, string password, List<User> users)
        {
            foreach (User user in users)
            {
                if (username == user.Name && password == user.Password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
