using Google.Cloud.Firestore;

namespace HavekrigerenApp.Models
{
    [FirestoreData]
    public class Category
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public int Id { get; set; }

        public Category(string name, int id)
        {
            Name = name;
            Id = id;
        }

        // Parameterless contructor for Firebase cus they stupid
        public Category()
        {
        }
    }
}
