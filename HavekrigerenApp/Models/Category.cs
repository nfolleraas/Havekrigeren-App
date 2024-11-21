using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Models
{
    public class Category
    {
        public string CategoryName { get; set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }

        public Category()
        {
        }
    }
}
