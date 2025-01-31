using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        public static async Task InitializeDatabase()
        {
            try
            {
                string fileName = "config.json";
                string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Config file not found, copying from package...");
                    try
                    {
                        using (var stream = await FileSystem.Current.OpenAppPackageFileAsync(fileName))
                        {
                            if (stream == null)
                            {
                                Console.WriteLine("Error: config.json was not found in the app package.");
                                return;
                            }

                            using (var destStream = File.Create(filePath))
                            {
                                await stream.CopyToAsync(destStream);
                            }
                        }
                        Console.WriteLine($"Config file copied to: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception occurred: {ex.Message}");
                    }
                }

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Error: config.json still not found after copying.");
                    return;
                }

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
                Console.WriteLine($"GOOGLE_APPLICATION_CREDENTIALS set to: {filePath}");

                // Read and parse the config file
                string json = File.ReadAllText(filePath);
                dynamic config = JsonConvert.DeserializeObject(json);

                if (config == null || config.project_id == null)
                {
                    Console.WriteLine("Error: config.json does not contain a valid project_id.");
                    return;
                }

                string projectId = config.project_id.ToString();
                Console.WriteLine($"Firebase Project ID: {projectId}");

                // Initialize Firebase Firestore
                _db = FirestoreDb.Create(projectId);
                Console.WriteLine("Firebase initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex.Message}");
            }
        }
    }
}
