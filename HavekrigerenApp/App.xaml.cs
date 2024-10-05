using FirebaseAdmin;
using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Reflection;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using static HavekrigerenApp.Pages.ViewAllCategoriesPage;

namespace HavekrigerenApp
{
    public partial class App : Application
    {
        public string ConfigFilePath { get; private set; }
        public static FirestoreDb db;

        public App()
        {
            InitializeComponent();

            InitializeFirebase();

            MainPage = new AppShell();
        }

        private async void InitializeFirebase()
        {
            try
            {
                // Define the filename
                string fileName = "config.json";

                // Get the app's data directory where files can be accessed
                string localPath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

                // Check if the file already exists in the app data directory
                if (!File.Exists(localPath))
                {
                    // Copy the file from the MAUI asset to the app data directory
                    using (var stream = await FileSystem.Current.OpenAppPackageFileAsync(fileName))
                    using (var destStream = File.Create(localPath))
                    {
                        await stream.CopyToAsync(destStream);
                    }
                }

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", localPath);

                // Read config.json
                string json = File.ReadAllText(localPath);
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

        public bool IsUserLoggedIn()
        {
            return Preferences.Default.Get("IsLoggedIn", false);
        }

        protected override void OnStart()
        {
            base.OnStart();

            Console.WriteLine("App is starting up");

            if (IsUserLoggedIn())
            {
                Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                Shell.Current.GoToAsync("///LoginPage");
            }
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            Console.WriteLine("App is going to sleep");
        }

        protected override void OnResume()
        {
            base.OnResume();

            Console.WriteLine("App is resuming");
        }

    }
}
