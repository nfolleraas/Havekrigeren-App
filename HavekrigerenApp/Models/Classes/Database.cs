using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Models.Classes
{
    public class Database
    {
        private static FirestoreDb _db;

        public static FirestoreDb Db
        {
            get { return _db; }
        }


        public static async void InitializeDatabase()
        {
            try
            {
                string fileName = "config.json";

                string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

                if (!File.Exists(filePath))
                {
                    using (var stream = await FileSystem.Current.OpenAppPackageFileAsync(fileName))
                    using (var destStream = File.Create(filePath))
                    {
                        await stream.CopyToAsync(destStream);
                    }
                }

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);

                // Extract project_id from json
                string json = File.ReadAllText(filePath);
                dynamic config = JsonConvert.DeserializeObject(json);
                string projectId = config.project_id.ToString();
                Console.WriteLine(projectId);

                _db = FirestoreDb.Create(projectId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex.Message}");
            }
        }
    }
}
