using HavekrigerenApp.Exceptions;
using HavekrigerenApp.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace HavekrigerenApp
{
    public partial class App : Application
    {
        public static string? ConnectionString { get; private set; }

        public App()
        {
            InitializeComponent();
            LoadConfig();

            MainPage = new AppShell();

        }

        private async void LoadConfig()
        {
            // Retrieve connectionstring
            using var stream = await FileSystem.OpenAppPackageFileAsync("config.json");
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            var jsonDoc = JsonDocument.Parse(json);
            ConnectionString = jsonDoc
                .RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("DBConnection")
                .GetString();

            // Try connecting to the database
            int retryCounter = 0;
            while (string.IsNullOrEmpty(ConnectionString))
            {
                try
                {
                    using var conn = new SqlConnection(ConnectionString);
                    await conn.OpenAsync();
                    break;
                }
                catch (SqlException ex)
                {
                    retryCounter++;
                    if (retryCounter == 5)
                        throw new TooManyAttemptsException("Could not connect to DB after 5 tries.", ex);

                    await Task.Delay(2000);
                }
            }
        }


        public bool IsUserLoggedIn()
        {
            return Preferences.Default.Get("IsLoggedIn", false);
        }

        protected async override void OnStart()
        {
            base.OnStart();

            Console.WriteLine("App is starting up");

            await Shell.Current.GoToAsync("///HomePage");

            /*
            if (IsUserLoggedIn())
            {
                await Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                await Shell.Current.GoToAsync("///LoginPage");
            }
            */
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
