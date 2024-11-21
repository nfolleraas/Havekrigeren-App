using Google.Cloud.Firestore;

namespace HavekrigerenApp.Models
{
    public class DatabaseRepository
    {
        private FirestoreDb db;
        public DatabaseRepository()
        {
            Database.InitializeDatabase();
            db = Database.Db;
        }

        public async Task<List<T>> GetAllAsync<T>(string collectionName) where T : class
        {
            try
            {
                var collection = db.Collection(collectionName);
                var snapshot = await collection.GetSnapshotAsync();
                var items = snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return new List<T>();
            }
        }

        public async Task AddAsync<T>(string collectionName, T item) where T : class
        {
            try
            {
                var collection = db.Collection(collectionName);
                await collection.AddAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding data: {ex.Message}");
            }
        }

        public async Task UpdateAsync<T>(string collectionName, string documentId, T item) where T : class
        {
            try
            {
                var document = db.Collection(collectionName).Document(documentId);
                await document.SetAsync(item, SetOptions.Overwrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating data: {ex.Message}");
            }
        }

        public async Task DeleteAsync(string collectionName, string documentId)
        {
            try
            {
                var document = db.Collection(collectionName).Document(documentId);
                await document.DeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting data: {ex.Message}");
            }
        }
    }
}
