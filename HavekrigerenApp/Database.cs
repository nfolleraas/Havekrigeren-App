using Google.Cloud.Firestore;
using Google.Type;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp
{
    public class Database
    {
        private static FirestoreDb db;

        public static async void InitializeFirebase()
        {
            try
            {
                // Define the filename
                string fileName = "config.json";

                // Get the app's data directory where files can be accessed
                string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

                // Check if the file already exists in the app data directory
                if (!File.Exists(filePath))
                {
                    // Copy the file from the MAUI asset to the app data directory
                    using (var stream = await FileSystem.Current.OpenAppPackageFileAsync(fileName))
                    using (var destStream = File.Create(filePath))
                    {
                        await stream.CopyToAsync(destStream);
                    }
                }

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);

                // Read config.json
                string json = File.ReadAllText(filePath);
                dynamic config = JsonConvert.DeserializeObject(json);
                string projectId = config.project_id.ToString();
                Console.WriteLine(projectId);

                // Extract project ID
                db = FirestoreDb.Create(projectId);
            }
            catch (Exception ex)
            {
                // Handle exceptions here (logging, alerts, etc.)
                Console.WriteLine($"Firebase initialization failed: {ex.Message}");
            }
        }

        public static async void AddDocument(string collectionName, Dictionary<string, object> data)
        {
            CollectionReference collRef = db.Collection(collectionName);

            try
            {
                await collRef.AddAsync(data);
                Console.WriteLine($"Data added successfully: {data} with ID: {collRef.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding data: {ex.Message}");
            }

        }

        public static async Task<List<T>> GetDocuments<T>(string collectionName) where T : new()
        {
            List<T> documentsList = new List<T>();
            CollectionReference collectionRef = db.Collection(collectionName);
            QuerySnapshot snap = await collectionRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snap.Documents)
            {
                if (document.Exists)
                {
                    Dictionary<string, object> documentDict = document.ToDictionary();
                    T documentObj = ConvertToObject<T>(documentDict);
                    if (documentObj != null)
                    {
                        documentsList.Add(documentObj);
                    }
                }
            }
            return documentsList;
        }

        // Helper method - Convert dictionary to appropriate datatype
        private static T ConvertToObject<T>(Dictionary<string, object> dict) where T : new()
        {
            if (typeof(T) == typeof(Job))
            {
                dict.TryGetValue("contactName", out object contactNameObj);
                string contactName = contactNameObj as string ?? "";

                dict.TryGetValue("address", out object addressObj);
                string address = addressObj as string ?? "";

                dict.TryGetValue("phoneNumber", out object phoneObj);
                string phoneNumber = phoneObj as string ?? "";

                dict.TryGetValue("category", out object categoryObj);
                string category = categoryObj as string ?? "";

                dict.TryGetValue("date", out object dateObj);
                string dateStr = dateObj as string ?? "";
                DateOnly date = DateOnly.ParseExact(dateStr, "dd/MM-yyyy");

                dict.TryGetValue("notes", out object notesObj);
                string notes = notesObj as string ?? "";

                return (T)(object)new Job(contactName, address, phoneNumber, category, date, notes);
            }
            else if (typeof(T) == typeof(Category))
            {
                dict.TryGetValue("categoryName", out object categoryNameObj);
                string categoryName = categoryNameObj as string ?? "";

                return (T)(object)new Category(categoryName);
            }
            else if ( typeof(T) == typeof(User))
            {
                dict.TryGetValue("name", out object nameObj);
                string name = nameObj as string ?? "";

                dict.TryGetValue("password", out object passwordObj);
                string password = passwordObj as string ?? "";

                return (T)(object)new User(name, password);
            }
            return default;
        }

        public static async void DeleteDocument(string collectionName, string documentKey, string documentValue)
        {
            CollectionReference collRef = db.Collection(collectionName);
            Query query = collRef.WhereEqualTo(documentKey, documentValue);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                await document.Reference.DeleteAsync();
            }

        }
    }
}
