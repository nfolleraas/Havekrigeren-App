using Google.Cloud.Firestore;
using HavekrigerenApp.Services;

namespace HavekrigerenApp.Models
{
    public class DatabaseRepository
    {
        private FirestoreDb db;
        private AlertService alertService;
        public DatabaseRepository()
        {
            Database.InitializeDatabase();
            db = Database.Db;
            alertService = new AlertService();
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

        public async Task DeleteAsync(string collectionName, string fieldName, string fieldValue)
        {
            try
            {
                var querySnapshot = await db.Collection(collectionName).WhereEqualTo(fieldName, fieldValue).GetSnapshotAsync();

                foreach(var document in querySnapshot.Documents)
                {
                    await document.Reference.DeleteAsync();
                }

                if (querySnapshot.Documents.Count == 0)
                {
                    await alertService.DisplayAlert("Fejl!", $"Kunne ikke finde den efterspurgte kategori \"{fieldValue}\".");
                }
            }
            catch (Exception ex)
            {
                await alertService.DisplayAlert("Fejl!", $"Fejlbesked: {ex.Message}");
            }
        }
    }
}
