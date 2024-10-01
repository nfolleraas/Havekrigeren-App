using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp;

public class User
{
    private string username;
    private string password;

    private static List<User> users = new List<User>();

    static User()
    {
        users.Add(new User { username = "Patrick", password = "1234" });
        users.Add(new User { username = "Martin", password = "1234" });
    }

    public bool Login(string username, string password)
    {
        foreach (var user in users)
        {
            if (username == user.username && password == user.password)
            {
                return true;
            }
        }
        return false;
    }
}
