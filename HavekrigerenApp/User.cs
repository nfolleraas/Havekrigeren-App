using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp;

public class User
{
    private string Username { get; set; }
    private string Password { get; set; }

    private static User[] users = new User[2];

    static User()
    {
        users[0] = new User { Username = "Patrick", Password = "1234"};
        users[1] = new User { Username = "Martin", Password = "1234" };
    }

    public bool Login(string username, string password)
    {
        foreach (User user in users)
        {
            if (username == user.Username && password == user.Password)
            {
                return true;
            }
        }
        return false;
    }
}
