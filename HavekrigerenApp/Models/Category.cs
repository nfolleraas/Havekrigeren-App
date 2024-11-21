using Google.Cloud.Firestore;

namespace HavekrigerenApp.Models
{
    [FirestoreData]
    public class Category
    {
        [FirestoreProperty]
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
        }

        // Parameterless contructor for Firebase cus they stupid
        public Category()
        {
        }
    }
}
