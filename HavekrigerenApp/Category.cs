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

namespace HavekrigerenApp
{
    public class Category
    {
        public string CategoryName { get; set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }

        public static void AddCategory(string categoryName)
        {
            CollectionReference collection = App.db.Collection("Categories");
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"categoryName", categoryName},
            };
            collection.AddAsync(data);
            Console.WriteLine($"Data added successfully: {data}");
        }

        public static async Task<List<Category>> GetCategories()
        {
            List<Category> categories = new List<Category>();
            CollectionReference catRef = App.db.Collection("Categories");
            QuerySnapshot snap = await catRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snap.Documents)
            {
                if (document.Exists)
                {
                    Dictionary<string, object> categoriesDict = document.ToDictionary();
                    if (categoriesDict.TryGetValue("categoryName", out object categoryName))
                    {
                        categories.Add(new Category(categoryName.ToString()));
                    }

                }
            }
            return categories;
        }

        public override string ToString()
        {
            return $"{CategoryName}";
        }
    }
}
